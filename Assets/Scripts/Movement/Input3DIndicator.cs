﻿using DG.Tweening;
using EchoOfTheTimes.Core;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class Input3DIndicator : InputIndicationAnimator
    {
        private Renderer _renderer;

        [SerializeField]
        private Color _defaultSphere;
        [SerializeField]
        private Color _splashSphere;
        [SerializeField]
        private Color _errorSplashSphere;

        protected override void Awake()
        {
            spawnedIndicator = Instantiate(inputIndicatorSettings.Indicator3DPrefab, Vector3.zero, Quaternion.identity, transform);
            spawnedIndicator.SetActive(false);

            _renderer = spawnedIndicator.GetComponent<Renderer>();
            _defaultSphere = _renderer.material.color;
        }

        public void ShowSuccessIndicator(Vertex at) => SpawnSphere(at.transform, _defaultSphere, _splashSphere);

        public void ShowErrorIndicator(Vertex at) => SpawnSphere(at.transform, _defaultSphere, _errorSplashSphere);

        private void SpawnSphere(Transform at, Color defaultColor, Color splash)
        {
            spawnedIndicator.SetActive(false);
            _renderer.material.color = defaultColor;
            spawnedIndicator.transform.localScale = Vector3.one * inputIndicatorSettings.DefaultRadius;

            Vector3 pos = at.GetComponentInChildren<IndicationPlaceholder>().transform.position;

            spawnedIndicator.transform.localPosition = pos;
            spawnedIndicator.SetActive(true);

            spawnedIndicator.transform.DOScale(inputIndicatorSettings.MaxRadius, inputIndicatorSettings.IndicatorDuration3D_sec)
                .OnComplete(() =>
                {
                    _renderer.material.DOColor(splash, inputIndicatorSettings.IndicatorColorDuration3D_sec);

                    spawnedIndicator.transform.DOScale(inputIndicatorSettings.DefaultRadius, inputIndicatorSettings.IndicatorDuration3D_sec)
                        .OnComplete(() =>
                        {
                            spawnedIndicator.SetActive(false);
                            _renderer.material.color = defaultColor;
                        });
                });
        }
    }
}