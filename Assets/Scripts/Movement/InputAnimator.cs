using DG.Tweening;
using EchoOfTheTimes.Core;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class InputAnimator : MonoBehaviour
    {
        public GameObject SpherePrefab;
        //public GameObject TouchIndicator;
        //[SerializeField]
        //private float _2dIndicatorDuration_sec = 0.5f;

        private GameObject _spawnedSphere;
        private Renderer _renderer;
        [SerializeField]
        private Color _defaultSphere = Color.black;
        [SerializeField]
        private Color _splashSphere = Color.white;
        [SerializeField]
        private float _defaultRadius = 0.3f;
        [SerializeField]
        private float _maxRadius = 0.6f;
        [SerializeField]
        private float _3dIndicatorDuration_sec = 0.1f;
        [SerializeField]
        private float _3dIndicatorColorDuration_sec = 0.05f;

        private void Awake()
        {
            _spawnedSphere = Instantiate(SpherePrefab, Vector3.zero, Quaternion.identity, transform);
            _spawnedSphere.SetActive(false);

            _renderer = _spawnedSphere.GetComponent<Renderer>();
            _defaultSphere = _renderer.material.color;
        }

        public void SpawnSphere(Vertex vertex)
        {
            //_spawnedSphere.transform.SetParent(vertex.transform);
            _spawnedSphere.SetActive(false);
            _spawnedSphere.transform.localScale = Vector3.one * _defaultRadius;

            Vector3 pos = vertex.GetComponent<BoxCollider>().center / 2f;

            _spawnedSphere.transform.localPosition = vertex.transform.position + pos;
            //_spawnedSphere.transform.localPosition = pos;
            _spawnedSphere.SetActive(true);

            _spawnedSphere.transform.DOScale(_maxRadius, _3dIndicatorDuration_sec)
                .OnComplete(() =>
                {
                    _renderer.material.DOColor(Color.white, _3dIndicatorColorDuration_sec);

                    _spawnedSphere.transform.DOScale(_defaultRadius, _3dIndicatorDuration_sec)
                        .OnComplete(() =>
                        {
                            _spawnedSphere.SetActive(false);
                            _renderer.material.color = _defaultSphere;
                        });
                });
        }

        public void SpawnScreenIndicator(Vector3 screenPosition)
        {
            //TouchIndicator.transform.position = screenPosition;

            //StartCoroutine(RunIndicator());
        }

        //private IEnumerator RunIndicator()
        //{
        //    TouchIndicator.SetActive(true);

        //    yield return new WaitForSeconds(_2dIndicatorDuration_sec);

        //    TouchIndicator.SetActive(false);
        //}

        //private void DespawnSphere()
        //{
        //    _spawnedSphere.SetActive(false);
        //}
    }
}