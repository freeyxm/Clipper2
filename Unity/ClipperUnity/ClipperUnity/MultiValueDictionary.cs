/*******************************************************************************
* Author    :  freeyxm                                                         *
* Date      :  2023.09.13                                                      *
* Website   :  https://github.com/freeyxm                                      *
* Copyright :                                                                  *
* Purpose   :  An multi-value dictionary implemention.                         *
* License   :                                                                  *
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Clipper2Lib
{
    public class MultiValueDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, List<TValue>> mDict = new Dictionary<TKey, List<TValue>>();
        private readonly Queue<List<TValue>> mCache = new Queue<List<TValue>>();
        private ValueCollection mValues;
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

        public ICollection<TValue> Values
        {
            get
            {
                if (mValues == null)
                    mValues = new ValueCollection(this);
                return mValues;
            }
        }

        public int Count => mCount;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            if (mDict.TryGetValue(key, out var list))
            {
                list.Add(value);
                mCount++;
            }
            else
            {
                if (mCache.Count > 0)
                    list = mCache.Dequeue();
                else
                    list = new List<TValue>(1);
                list.Add(value);
                mDict.Add(key, list);
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

        public bool ContainsValue(TValue value)
        {
            for (var e = mDict.GetEnumerator(); e.MoveNext();)
            {
                if (e.Current.Value.Contains(value))
                    return true;
            }
            return false;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (mDict.TryGetValue(item.Key, out var list))
            {
                return list.Contains(item.Value);
            }
            return false;
        }

        public bool Remove(TKey key)
        {
            if (mDict.TryGetValue(key, out var list))
            {
                mCount -= list.Count;
                list.Clear();
                mDict.Remove(key);
                mCache.Enqueue(list);
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (mDict.TryGetValue(item.Key, out var list))
            {
                var index = list.LastIndexOf(item.Value);
                var exist = index >= 0;
                if (exist)
                {
                    list.RemoveAt(index);
                    mCount--;
                }
                if (list.Count == 0)
                {
                    mDict.Remove(item.Key);
                    mCache.Enqueue(list);
                }
                return exist;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (mDict.TryGetValue(key, out var list))
            {
                if (list.Count > 0)
                {
                    value = list[list.Count - 1];
                    return true;
                }
            }
            value = default(TValue);
            return false;
        }

        public bool TryPopValue(TKey key, out TValue value)
        {
            if (mDict.TryGetValue(key, out var list))
            {
                int index = list.Count - 1;
                {
                    value = list[index];
                    list.RemoveAt(index);
                    mCount--;
                }
                if (list.Count == 0)
                {
                    mDict.Remove(key);
                    mCache.Enqueue(list);
                }
                return true;
            }
            value = default(TValue);
            return false;
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
                var list = e.Current.Value;
                for (int i = 0; i < list.Count; i++)
                {
                    yield return new KeyValuePair<TKey, TValue>(key, list[i]);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var e = mDict.GetEnumerator(); e.MoveNext();)
            {
                var key = e.Current.Key;
                var list = e.Current.Value;
                for (int i = 0; i < list.Count; i++)
                {
                    yield return new KeyValuePair<TKey, TValue>(key, list[i]);
                }
            }
        }

        private class ValueCollection : ICollection<TValue>, ICollection
        {
            private MultiValueDictionary<TKey, TValue> mDict;

            public ValueCollection(MultiValueDictionary<TKey, TValue> dictionay)
            {
                mDict = dictionay;
            }

            public int Count => mDict.Count;

            public bool IsReadOnly => true;

            public bool IsSynchronized => false;

            public object SyncRoot => ((ICollection)mDict).SyncRoot;

            public void Add(TValue item)
            {
                throw new NotSupportedException("NotSupported_ValueCollectionSet");
            }

            public bool Remove(TValue item)
            {
                throw new NotSupportedException("NotSupported_ValueCollectionSet");
            }

            public void Clear()
            {
                throw new NotSupportedException("NotSupported_ValueCollectionSet");
            }

            public bool Contains(TValue item)
            {
                return mDict.ContainsValue(item);
            }

            public void CopyTo(TValue[] array, int index)
            {
                for (var e = GetEnumerator(); e.MoveNext();)
                {
                    array[index++] = e.Current;
                }
            }

            public void CopyTo(Array array, int index)
            {
                CopyTo(array as TValue[], index);
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
