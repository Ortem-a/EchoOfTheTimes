using EchoOfTheTimes.Core;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class VerticesBlocker : MonoBehaviour
    {
        private LevelStateMachine _stateMachine;

        public List<BlockedVertex> BlockedVertices;

        public void Initialize()
        {
            _stateMachine = GameManager.Instance.StateMachine;
        }

        public void Block()
        {
            if (BlockedVertices != null && BlockedVertices.Count > 0)
            {
                SetActiveToAll(BlockedVertices, true);

                var vertices = BlockedVertices.FindAll((x) => x.StateId == _stateMachine.GetCurrentStateId());
                SetActiveToAll(vertices, false);
            }
        }

        public void Unblock()
        {
            if (BlockedVertices != null && BlockedVertices.Count > 0)
            {
                var vertices = BlockedVertices.FindAll((x) => x.StateId == _stateMachine.GetCurrentStateId());
                SetActiveToAll(vertices, true);
            }
        }

        private void SetActiveToAll(List<BlockedVertex> vertices, bool isActive)
        {
            if (vertices != null && vertices.Count > 0)
            {
                foreach (var blocked in vertices)
                {
                    blocked.Vertex.gameObject.SetActive(isActive);
                }
            }
        }
    }
}