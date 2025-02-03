using Systems.Movement;
using UnityEngine;

namespace Systems.Leveling
{
    public class FinishButton : MonoBehaviour, ISpecialVertex
    {
        public SpecialVertexType Type => SpecialVertexType.Button;

        public void OnEnter(IUnit unit)
        {
            Debug.Log($"[FINISH BUTTON] {unit} enter");
        }

        public void OnExit(IUnit unit)
        {
            throw new System.NotImplementedException();
        }
    }
}