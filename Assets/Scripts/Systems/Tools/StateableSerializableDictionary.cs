using System.Linq;
using Systems.Leveling;
using UnityEngine;

namespace Systems.Tools
{
    [System.Serializable]
    public class StateableSerializableDictionary : DummySerializableDictionary<int, StateOption>
    {
        public void AddOrUpdate(int stateId, Transform target)
        {
            var newOption = new StateOption()
            {
                Target = target,
                LocalPosition = target.localPosition,
                LocalRotation = target.localRotation,
                LocalScale = target.localScale,
            };

            if (TryGetKey(stateId, out int itemIndex))
            {
                Items[itemIndex].Value = newOption;
            }
            else
            {
                var newElement = new DummySerializableDictionaryItem<int, StateOption>[1]
                {
                    new DummySerializableDictionaryItem<int, StateOption>
                    {
                        Key = stateId,
                        Value = newOption
                    }
                };

                Items = Items.Concat(newElement).ToArray();
            }
        }

        public bool TryGetValue(int key, out StateOption option)
        {
            if (TryGetKey(key, out int itemIndex))
            {
                option = Items[itemIndex].Value;
                return true;
            }

            option = null;
            return false;
        }

        private bool TryGetKey(int key, out int index)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].Key == key)
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }
    }
}