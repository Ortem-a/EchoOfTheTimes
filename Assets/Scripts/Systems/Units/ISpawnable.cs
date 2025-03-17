using Systems.Movement;
using Systems.Settings;

namespace Systems.Units
{
    public interface ISpawnable
    {
        public IUnit Spawn(UnitSettingsScriptableObject unitSettings, Vertex at, GraphVisibility graph);
    }
}