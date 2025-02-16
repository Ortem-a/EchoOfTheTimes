using System;
using System.Collections.Generic;

namespace Systems.Tools
{
    [System.Serializable]
    public class DummySerializableDictionary<TKey, TValue>
    {
        public DummySerializableDictionaryItem<TKey, TValue>[] Items;

        public DummySerializableDictionary()
        {
            Items = Array.Empty<DummySerializableDictionaryItem<TKey, TValue>>();
        }

        public virtual Dictionary<TKey, TValue> ToDictionary()
        {
            var newDictionary = new Dictionary<TKey, TValue>();

            foreach (var item in Items)
            {
                newDictionary.Add(item.Key, item.Value);
            }

            return newDictionary;
        }
    }
}