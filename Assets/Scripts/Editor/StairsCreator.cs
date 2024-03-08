using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EchoOfTheTimes.Editor
{
    public class StairsCreator : MonoBehaviour
    {
        [Header("Settings")]
        public int StairsNumber;
        public float TopHeight;
        public float StairWidth;

        [Space]
        [InspectorButton(nameof(CreateOrUpdate))]
        public bool IsCreateOrUpdate;
        [Space]

        public GameObject StairPrefab;

        private List<GameObject> _spawnedStairs;

        public void CreateOrUpdate()
        {
            Despawn();

            _spawnedStairs = new List<GameObject>();

            for (int i = 0; i < StairsNumber; i++) 
            {
                var spawned = Instantiate(StairPrefab, transform);

                float step = TopHeight / (float)StairsNumber;

                spawned.transform.localScale = new Vector3(1f, 1f ,StairWidth);
                spawned.transform.localPosition = new Vector3(0f, step, StairWidth / 4f) * i;

                _spawnedStairs.Add(spawned);
            }
        }

        private void Despawn()
        {
            if (_spawnedStairs != null) 
            {
                foreach (var spawned in _spawnedStairs)
                {
                    DestroyImmediate(spawned);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Vector3 top = 
                _spawnedStairs.Last().transform.position + Vector3.up * StairPrefab.transform.localScale.y;

            Gizmos.DrawSphere(top, 0.5f);

        }
    }
}