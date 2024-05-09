using EchoOfTheTimes.Core;
using System.Collections;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class InputAnimator : MonoBehaviour
    {
        public GameObject SpherePrefab;
        public GameObject TouchIndicator;

        private GameObject _spawnedSphere;

        public void SpawnSphere(Vertex vertex)
        {
            if (_spawnedSphere != null)
            {
                DespawnSphere();
            }

            _spawnedSphere = Instantiate(SpherePrefab, vertex.transform.position, Quaternion.identity, transform);
        }

        public void SpawnScreenIndicator(Vector3 screenPosition)
        {
            TouchIndicator.transform.position = screenPosition;

            StartCoroutine(RunIndicator());
        }

        private IEnumerator RunIndicator()
        {
            TouchIndicator.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            TouchIndicator.SetActive(false);
        }

        private void DespawnSphere()
        {
            DestroyImmediate(_spawnedSphere);
        }
    }
}