using EchoOfTheTimes.Collectables;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemSettings", order = 10)]
public class ItemSettingsScriptableObject : ScriptableObject
{
    [field: SerializeField]
    public int Id { get; private set; }
    [field: SerializeField]
    public CollectableStatusType Status { get; private set; }
    [field: SerializeField]
    public Item Prefab { get; private set; }
}
