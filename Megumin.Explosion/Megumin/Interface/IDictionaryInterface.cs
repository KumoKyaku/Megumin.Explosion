using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    public interface IDictionary<TKey1, TKey2, TValue> : ICollection<(TKey1 Key1, TKey2 Key2, TValue Value)>, IEnumerable<(TKey1 Key1, TKey2 Key2, TValue Value)>, IEnumerable
    {
        TValue this[TKey1 key1]
        {
            get;
        }

        TValue this[TKey2 key2]
        {
            get;
        }

        TValue this[TKey1 key1,TKey2 key2]
        {
            set;
        }

        ICollection<TKey1> Key1s
        {
            get;
        }

        ICollection<TKey2> Key2s
        {
            get;
        }

        ICollection<TValue> Values
        {
            get;
        }

        void Add(TKey1 key1,TKey2 key2, TValue value);

        bool ContainsKey(TKey1 key1);
        bool ContainsKey(TKey2 key2);

        bool Remove(TKey1 key1, TKey2 key2);

        bool TryGetValue(TKey1 key, out TValue value);
        bool TryGetValue(TKey2 key, out TValue value);
        void RemoveAll(Func<(TKey1 Key1, TKey2 Key2, TValue Value), bool> predicate);
    }
}
