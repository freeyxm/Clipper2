using System;
using System.Collections;
using System.Collections.Generic;

namespace Clipper2Lib
{
    public class MultiValueDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, object> mDict = new Dictionary<TKey, object>();
        private readonly Queue<List<TValue>> mCache = new Queue<List<TValue>>();
        private int mCount = 0;

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var _value))
                    return _value;
                else
                    throw new KeyNotFoundException();
            }
            set => Add(key, value);
        }

        public ICollection<TKey> Keys => mDict.Keys;

        public ICollection<TValue> Values => new ValueCollection(this);

        public int Count => mCount;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            if (mDict.TryGetValue(key, out var _value))
            {
                if (_value is List<TValue>)
                {
                    var list = _value as List<TValue>;
                    list.Add(value);
                    mCount++;
                }
                else
                {
                    List<TValue> list;
                    if (mCache.Count > 0)
                        list = mCache.Dequeue();
                    else
                        list = new List<TValue>(2);
                    list.Add((TValue)_value);
                    list.Add(value);
                    mDict[key] = list;
                    mCount++;
                }
            }
            else
            {
                mDict.Add(key, value);
                mCount++;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            mDict.Clear();
            mCache.Clear();
            mCount = 0;
        }

        public bool ContainsKey(TKey key)
        {
            return mDict.ContainsKey(key);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (mDict.TryGetValue(item.Key, out var _value))
            {
                if (_value is List<TValue>)
                {
                    var list = _value as List<TValue>;
                    return list.Contains(item.Value);
                }
                else
                {
                    return _value.Equals(item.Value);
                }
            }
            return false;
        }

        public bool Remove(TKey key)
        {
            if (mDict.TryGetValue(key, out var _value))
            {
                if (_value is List<TValue>)
                {
                    var list = _value as List<TValue>;
                    mCount -= list.Count;
                    list.Clear();
                    mDict.Remove(key);
                    mCache.Enqueue(list);
                    return true;
                }
                else
                {
                    mDict.Remove(key);
                    mCount--;
                    return true;
                }
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (mDict.TryGetValue(item.Key, out var _value))
            {
                if (_value is List<TValue>)
                {
                    var list = _value as List<TValue>;
                    var index = list.LastIndexOf(item.Value);
                    if (index >= 0)
                    {
                        list.RemoveAt(index);
                        mCount--;
                    }
                    if (list.Count == 0)
                    {
                        mDict.Remove(item.Key);
                        mCache.Enqueue(list);
                    }
                    return index >= 0;
                }
                else if (_value.Equals(item.Value))
                {
                    mDict.Remove(item.Key);
                    mCount--;
                    return true;
                }
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (mDict.TryGetValue(key, out var _value))
            {
                if (_value is List<TValue>)
                {
                    var list = _value as List<TValue>;
                    if (list.Count > 0)
                    {
                        value = list[list.Count - 1];
                        return true;
                    }
                    else
                    {
                        value = default(TValue);
                        return false;
                    }
                }
                else
                {
                    value = (TValue)_value;
                    return true;
                }
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            for (var e = GetEnumerator(); e.MoveNext();)
            {
                array[index++] = e.Current;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (var e = mDict.GetEnumerator(); e.MoveNext();)
            {
                var key = e.Current.Key;
                var value = e.Current.Value;
                if (value is List<TValue>)
                {
                    var list = (List<TValue>)value;
                    for (int i = 0; i < list.Count; i++)
                    {
                        yield return new KeyValuePair<TKey, TValue>(key, list[i]);
                    }
                }
                else
                {
                    yield return new KeyValuePair<TKey, TValue>(key, (TValue)value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var e = mDict.GetEnumerator(); e.MoveNext();)
            {
                var key = e.Current.Key;
                var value = e.Current.Value;
                if (value is List<TValue>)
                {
                    var list = (List<TValue>)value;
                    for (int i = 0; i < list.Count; i++)
                    {
                        yield return new KeyValuePair<TKey, TValue>(key, list[i]);
                    }
                }
                else
                {
                    yield return new KeyValuePair<TKey, TValue>(key, (TValue)value);
                }
            }
        }

        private class ValueCollection : ICollection<TValue>
        {
            private MultiValueDictionary<TKey, TValue> mDict;

            public ValueCollection(MultiValueDictionary<TKey, TValue> dictionay)
            {
                mDict = dictionay;
            }

            public int Count => mDict.Count;

            public bool IsReadOnly => true;

            public void Add(TValue item)
            {
            }

            public bool Remove(TValue item)
            {
                return false;
            }

            public void Clear()
            {
            }

            public bool Contains(TValue item)
            {
                for (var e = GetEnumerator(); e.MoveNext();)
                {
                    if (e.Current.Equals(item))
                        return true;
                }
                return false;
            }

            public void CopyTo(TValue[] array, int index)
            {
                for (var e = GetEnumerator(); e.MoveNext();)
                {
                    array[index++] = e.Current;
                }
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                for (var e = mDict.GetEnumerator(); e.MoveNext();)
                {
                    yield return e.Current.Value;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                for (var e = mDict.GetEnumerator(); e.MoveNext();)
                {
                    yield return e.Current.Value;
                }
            }
        }
    }
}
