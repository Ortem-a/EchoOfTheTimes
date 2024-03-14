using EchoOfTheTimes.LevelStates;
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

        public GameObject StairPrefab;
        public GameObject VertexPrefab;

        private List<GameObject> _spawnedStairs;

        public void CreateOrUpdate()
        {
            AddExistingStairsIfNeed();

            Despawn();

            _spawnedStairs = new List<GameObject>();

            for (int i = 0; i < StairsNumber; i++)
            {
                var spawned = Instantiate(StairPrefab, transform);

                float step = TopHeight / (float)StairsNumber;

                spawned.name = $"Stair_{i}";
                spawned.transform.localScale = new Vector3(1f, 1f, StairWidth);
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

        private void AddExistingStairsIfNeed()
        {
            if (_spawnedStairs == null)
            {
                var stairs = GetComponentsInChildren<Stateable>();
                if (stairs != null)
                {
                    _spawnedStairs = new List<GameObject>();

                    foreach (var stair in stairs)
                    {
                        _spawnedStairs.Add(stair.gameObject);
                    }
                }
            }
        }

#warning »—œ–¿¬»“‹ ›“” œ¿–¿ÿ” Õ¿ Œœ“»Ã¿À‹Õ”ﬁ
        public void AddDefaultStates()
        {
            AddExistingStairsIfNeed();

            Stateable[] stairs = transform.GetComponentsInChildren<Stateable>();

            for (int i = 0; i < stairs.Length; i++)
            {
                stairs[i].CurrentStateId = 0;
                stairs[i].SetOrUpdateParamsToState();
            }

            var copy = new Vector3[stairs.Length];
            for (int i = 0; i < stairs.Length; i++)
            {
                copy[i] = stairs[i].transform.position;
            }

            for (int i = 0; i < stairs.Length; i++)
            {
                stairs[i].CurrentStateId = 1;

                stairs[i].transform.position = new Vector3(
                    stairs[i].transform.position.x,
                    copy[copy.Length - 1 - i].y,
                    stairs[i].transform.position.z);

                stairs[i].SetOrUpdateParamsToState();
            }

            for (int i = 0; i < stairs.Length; i++)
            {
                stairs[i].CurrentStateId = 0;
                stairs[i].TransformObjectByState();
            }

            for (int i = 0; i < stairs.Length; i++)
            {
                stairs[i].CurrentStateId = 2;

                stairs[i].transform.position = new Vector3(
                    stairs[i].transform.position.x,
                    stairs[0].transform.position.y,
                    stairs[i].transform.position.z);

                stairs[i].SetOrUpdateParamsToState();
            }

            for (int i = 0; i < stairs.Length; i++)
            {
                stairs[i].CurrentStateId = 0;
                stairs[i].TransformObjectByState();
            }

            for (int i = 0; i < stairs.Length; i++)
            {
                stairs[i].CurrentStateId = 3;

                stairs[i].transform.position = new Vector3(
                    stairs[i].transform.position.x,
                    stairs[^1].transform.position.y,
                    stairs[i].transform.position.z);

                stairs[i].SetOrUpdateParamsToState();
            }
        }

        public void AddDefaultVertices()
        {
            Stateable[] stairs = transform.GetComponentsInChildren<Stateable>();

            for (int i = 1; i < stairs.Length; i += 3)
            {
                var obj = Instantiate(VertexPrefab, stairs[i].transform);
                obj.transform.localPosition = Vector3.up * StairPrefab.transform.localScale.y + Vector3.up * 1.5f;
            }
        }
    }
}