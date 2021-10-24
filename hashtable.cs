using System;

namespace Collection
{

    class HashTable<K, V>
    {
        public class Record
        {
            public K key;
            public V value;

            public bool isEmpty = true;
            public long hash = 0;

            public Record nodeL = null;
            public Record nodeR = null;

            public Record(K key, V value, long hash)
            {
                this.key = key;
                this.value = value;
                this.hash = hash;
                isEmpty = false;
            }

            public void forEach(Action<Record> action)
            {
                action(this);

                if (nodeL != null)
                    action(nodeL);

                if (nodeR != null)
                    action(nodeR);
            }

        }

        private int capacity;
        private Record[] table;

        public HashTable()
        {
            capacity = 50; // default = 50
            empty();
        }

        public HashTable(int capacity)
        {
            this.capacity = capacity;
            empty();
        }

        public void empty()
        {
            table = new Record[capacity];
        }

        private long hashcode(K key)
        {
            char[] code = key.ToString().ToCharArray();
            long hash = 0;
            for (int i = 0; i < code.Length; i++) 
            {
                hash += ((byte)code[i]) << i;
            }
            return hash;
        }

        private int locate(long hash)
        {
            return (int) (hash % capacity);
        }

        private int locate(K key)
        {
            return (int) (hashcode(key) % capacity);
        }

        /*  public void addAlt(K key, V value)
            {
                findAndPerform(key
                    ,record =>
                    {
                        record.value = value;
                    }
                    ,record =>
                    {
                        record = new Record(key, value, hashcode(key));
                        record.key = key;
                        record.value = value;
                        record.hash = hashcode(key);
                    }
                );
            }
        */

        public void add(K key, V value) // ! unable to create Record instance with findAndPerform Func 
        {
            Record record = new Record(key, value, hashcode(key));
            Record temp;
            int location = locate(record.hash);
            if (table[location] == null)
            {
                table[location] = record;
            }
            else
            {
                if (table[location].key.Equals(key)) 
                {
                    table[location].value = value;
                    table[location].isEmpty = false;
                    return;
                }

                temp = table[location];
                do
                {
                    if (record.hash < temp.hash)
                    {
                        if (temp.nodeL == null)
                        {
                            temp.nodeL = record;
                            return;
                        }
                        temp = temp.nodeL;
                    }
                    else
                    {
                        if (temp.nodeR == null)
                        {
                            temp.nodeR = record;
                            return;
                        }
                        temp = temp.nodeR;
                    }

                } while (!temp.key.Equals(key));
                temp.value = value;
                temp.isEmpty = false;
            }
        }

        public void replace(K key, V value)
        {
            findAndPerform(key
                , record =>
                {
                    if (!record.isEmpty)
                        record.value = value;
                }
                , record =>
                {

                }
            );
        }

        public void replace(K key, V oldValue, V newValue)
        {
            findAndPerform(key
                , record =>
                {
                    if (record.value.Equals(oldValue))
                        record.value = newValue;
                }
                , record =>
                {

                }
            );
        }

        public V get(K key)
        {
            V result = default(V);
            findAndPerform(key
                , record =>
                {
                    if(!record.isEmpty)
                        result = record.value;
                }
                , record =>
                {

                }
            );
            return result;
        }

        public V getOrDefault(K key, V defaultValue)
        {
            V result = defaultValue;
            findAndPerform(key
                , record =>
                {
                    if (!record.isEmpty)
                        result = record.value;
                }
                , record =>
                {

                }
            );
            return result;
        }

        public void remove(K key)
        {
            findAndPerform(key
                , record => 
                {
                    record.isEmpty = true;
                    record.value = default(V);
                }
                , record =>
                {
                    
                }
            );
        }

        public bool containsKey(K key)
        {
            bool output = false;
            findAndPerform(key
                , record =>
                {
                    if(!record.isEmpty)
                        output = true;
                }
                , record =>
                {
                    
                }
            );
            return output;
        }

        public bool containsValue(V value)
        {
            bool output = false;
            forEach(record => 
            {
                if (record.value.Equals(value))
                    output = true;
            });
            return output;
        }

        public bool isEmpty()
        {
            return size() > 0;
        }

        public int size()
        {
            int output = 0;
            forEach(record =>
            {
                if(!record.isEmpty)
                    output++;
            });
            return output;
        }

        public Record[] toArray() 
        {
            Record[] output = new Record[size()];
            int count = 0;
            forEach(record =>
            {
                if (!record.isEmpty) 
                {
                    output[count] = record;
                    count++;
                }                   
            });
            return output;
        }

        public void compute(K key, Func<K, V, V> mappingFunction)
        {
            findAndPerform(key
                , record =>
                {
                    record.value = mappingFunction(record.key, record.value);
                }
                , record =>
                {

                }
            );
        }


        public void findAndPerform(K key, Action<Record> nodeExist
                                        , Action<Record> nodeEmpty)
        {
            long hash = hashcode(key);
            Record temp;
            int location = locate(hash);
            if (table[location] == null)
            {
                nodeEmpty(table[location]);
            }
            else
            {
                if (table[location].key.Equals(key))
                {
                    nodeExist(table[location]);
                }
                else
                {
                    temp = table[location];
                    do
                    {
                        if (hash < temp.hash)
                        {
                            if (temp.nodeL == null)
                            {
                                nodeEmpty(temp);
                            }
                            temp = temp.nodeL;
                        }
                        else
                        {
                            if (temp.nodeR == null)
                            {
                                nodeEmpty(temp);
                            }
                            temp = temp.nodeR;
                        }

                    } while (!temp.key.Equals(key));
                    nodeExist(temp);
                }
            }
        }

        public Record findAndPerform(K key, Func<Record, Record> nodeExist
                                          , Func<Record, Record> nodeEmpty)
        {
            long hash = hashcode(key);
            Record temp;
            int location = locate(hash);
            if (table[location] == null)
            {
                return nodeEmpty(table[location]);
            }
            else
            {
                if (table[location].key.Equals(key))
                {
                    return nodeExist(table[location]);
                }
                else
                {
                    temp = table[location];
                    do
                    {
                        if (hash < temp.hash)
                        {
                            if (temp.nodeL == null)
                            {
                                return nodeEmpty(temp);
                            }
                            temp = temp.nodeL;
                        }
                        else
                        {
                            if (temp.nodeR == null)
                            {
                                return nodeEmpty(temp);
                            }
                            temp = temp.nodeR;
                        }

                    } while (!temp.key.Equals(key));

                    return nodeExist(temp);
                }
            }
        }

        public void forEach(Action<Record> action) 
        {
            foreach (Record record in table) 
            {
                if(record != null)
                    record.forEach(action);
            }
        }

    }
}
