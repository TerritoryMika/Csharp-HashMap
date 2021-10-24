![structure](https://i.ibb.co/KbzzQVk/hadh.png)
# C-HashMap
C#'s Dictionary has no default null handle. Therefore, I implemented similar ideas from Java HashMap

## Method

#### void add(K key, V value)
#### void replace(K key, V value)
#### void replace(K key, V oldValue, V newValue)
#### V get(K key)
#### V getOrDefault(K key, V defaultValue)
#### void remove(K key)
#### bool containsKey(K key)
#### bool containsValue(V value)
#### bool isEmpty()
#### int size()
#### Record[] toArray()
#### void findAndPerform(K key, Action<Record> nodeExist, Action<Record> nodeEmpty)
#### Record findAndPerform(K key, Func<Record, Record> nodeExist, Func<Record, Record> nodeEmpty)
#### void forEach(Action<Record> action)
