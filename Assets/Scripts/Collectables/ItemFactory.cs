using EchoOfTheTimes.Core;
using System;
using UnityEngine;

namespace EchoOfTheTimes.Collectables
{
    [Serializable]
    public class ItemFactory
    {
        [SerializeField]
        private ItemPlaceholder _at;

        [SerializeField]
        private ItemSettingsScriptableObject _preset;

        public Item Create()
        {
            var spawnedItem = GameObject.Instantiate(_preset.Prefab, _at.transform);
            spawnedItem.Spawn(_preset.Id, _preset.Status);

            return spawnedItem;
        }
    }
}