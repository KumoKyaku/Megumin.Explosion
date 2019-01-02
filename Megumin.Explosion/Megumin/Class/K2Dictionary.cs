using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 不是线程安全的
    /// </summary>
    /// <typeparam name="TKey1"></typeparam>
    /// <typeparam name="TKey2"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Obsolete("多线程BUG",true)]
    public class K2Dictionary<TKey1, TKey2, TValue> : IDictionary<TKey1,TKey2,TValue>
    {
        readonly Dictionary<TKey1, TValue> dic1 = new Dictionary<TKey1, TValue>();
        readonly Dictionary<TKey2, TValue> dic2 = new Dictionary<TKey2, TValue>();
        IEnumerable<(TKey1, TKey2, TValue Value)> Enumerator { get; }

        public K2Dictionary()
        {
            //todo 此处多线程BUG   需要仿照Dictionary重写一个结构体，防止迭代时分配
            Enumerator = from item1 in dic1
                         from item2 in dic2
                         select (item1.Key, item2.Key, item2.Value);
        }

        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            if (key1 == null || key2 == null)
            {
                throw new ArgumentNullException();
            }
            if (dic1.ContainsKey(key1) || dic2.ContainsKey(key2))
            {
                throw new ArgumentException();
            }

            dic1.Add(key1, value);
            dic2.Add(key2, value);
        }

        public void Clear()
        {
            dic1.Clear();
            dic2.Clear();
        }

        public bool ContainsKey(TKey1 key1) => dic1.ContainsKey(key1);

        public bool ContainsKey(TKey2 key2) => dic2.ContainsKey(key2);

        public bool ContainsValue(TValue value) => dic1.ContainsValue(value);

        public TValue this[TKey1 key1, TKey2 key2]
        {
            set
            {
                dic1[key1] = value;
                dic2[key2] = value;
            }
        }

        public TValue this[TKey1 key1]
        {
            get
            {
                return dic1[key1];
            }
        }

        public TValue this [TKey2 key2]
        {
            get
            {
                return dic2[key2];
            }
        }

        public IEnumerator<(TKey1 Key1, TKey2 Key2, TValue Value)> GetEnumerator()
        {
            foreach (var item in Enumerator)
            {
                yield return item;
            }
        }

        readonly List<(TKey1 Key1, TKey2 Key2, TValue Value)> removelist = new List<(TKey1 Key1, TKey2 Key2, TValue Value)>();
        public void RemoveAll(Func<(TKey1 Key1,TKey2 Key2,TValue Value), bool> predicate)
        {
            lock (this)
            {
                removelist.Clear();

                foreach (var item in this)
                {
                    if (predicate(item))
                    {
                        removelist.Add(item);
                    }
                }

                foreach (var item in removelist)
                {
                    Remove(item.Key1, item.Key2);
                }

                removelist.Clear();
            }
        }

        public bool Remove(TKey1 key1,TKey2 key2)
        {
            return dic1.Remove(key1) & dic2.Remove(key2);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public ICollection<TKey1> Key1s => dic1.Keys;
        public ICollection<TKey2> Key2s => dic2.Keys;
        public ICollection<TValue> Values => dic1.Values;

        public bool TryGetValue(TKey1 key1, out TValue value) => dic1.TryGetValue(key1, out value);

        public bool TryGetValue(TKey2 key2, out TValue value) => dic2.TryGetValue(key2, out value);

        public void Add((TKey1 Key1, TKey2 Key2, TValue Value) item) => Add(item.Key1, item.Key2, item.Value);

        public bool Contains((TKey1 Key1, TKey2 Key2, TValue Value) item) => ContainsKey(item.Key1);

        public void CopyTo((TKey1 Key1, TKey2 Key2, TValue Value)[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove((TKey1 Key1, TKey2 Key2, TValue Value) item) => Remove(item.Key1, item.Key2);

        public int Count => dic1.Count;
        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey1, TValue>>)dic1).IsReadOnly &&
            ((ICollection<KeyValuePair<TKey2, TValue>>)dic2).IsReadOnly;
    }
}
