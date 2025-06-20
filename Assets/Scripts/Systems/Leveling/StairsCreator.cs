using System.Collections.Generic;
using Systems.Tools;
using UnityEngine;

namespace Systems.Leveling
{
    public class StairsCreator : MonoBehaviour
    {
        private float _stairHeight;

        public List<StairsStates> StairsStates;

        [HideInInspector]
        public Stair[] Stairs;

        private List<Vector3> _flatBottomPositions;

        private Vector3 _realScale;

        public void Initialize(float stairHeight, List<Vector3> flatBottomPositions)
        {
            _stairHeight = stairHeight;
            _flatBottomPositions = flatBottomPositions;
        }

        private void Awake()
        {
            Stairs = GetOrFindStairs();

            //SetRealSize();

            _flatBottomPositions = new List<Vector3>();

            Vector3 position = Stairs[0].transform.localPosition;

            _flatBottomPositions.Add(position);

            for (int i = 1; i < Stairs.Length; i++)
            {
                position += Vector3.forward * _realScale.z;
                _flatBottomPositions.Add(position);
            }
        }

        public List<StateOption> GetFlatBottomPositions()
        {
            return PositionsToStateOptions(_flatBottomPositions);
        }

        public List<StateOption> GetFlatTopPositions()
        {
            List<Vector3> positions = _flatBottomPositions;
            Vector3 startPosition = _flatBottomPositions.Count * _stairHeight * Vector3.up;

            for (int i = 0; i < _flatBottomPositions.Count; i++)
            {
                positions[i] += startPosition;
            }

            return PositionsToStateOptions(positions);
        }

        public List<StateOption> GetStartBottomPositions()
        {
            List<Vector3> positions = _flatBottomPositions;
            Vector3 startPosition;

            for (int i = 0; i < _flatBottomPositions.Count; i++)
            {
                startPosition = (i + 1) * _stairHeight * Vector3.up;

                positions[i] += startPosition;
            }

            return PositionsToStateOptions(positions);
        }

        public List<StateOption> GetStartTopPositions()
        {
            List<Vector3> positions = _flatBottomPositions;
            Vector3 startPosition;

            for (int i = 0; i < _flatBottomPositions.Count; i++)
            {
                startPosition = (_flatBottomPositions.Count - i) * _stairHeight * Vector3.up;

                positions[i] += startPosition;
            }

            return PositionsToStateOptions(positions);
        }

        private List<StateOption> PositionsToStateOptions(List<Vector3> positions)
        {
            var options = new List<StateOption>();
            
            for (int i = 0; i < Stairs.Length; i++)
            {
                options.Add(new StateOption()
                    {
                        LocalPosition = transform.TransformPoint(positions[i]),
                        LocalRotation = Stairs[i].transform.rotation,
                        LocalScale = Stairs[i].transform.localScale
                    });
            }

            return options;
        }

        private Stair[] GetOrFindStairs()
        {
            Stairs ??= GetComponentsInChildren<Stair>();

            return Stairs;
        }

        private void SetRealSize()
        {
            _realScale = Stairs[0].GetComponent<MeshFilter>().sharedMesh.bounds.size;

            _stairHeight = _realScale.y / 6f;
        }
    }
}