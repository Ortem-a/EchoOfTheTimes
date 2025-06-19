using DG.Tweening;
using System;
using Systems.Movement;
using UnityEngine;

namespace Systems.Units
{
    public class Teleportable : MonoBehaviour, ITeleportable
    {
        public float TeleportDisappearDuration_sec { get; private set; } = 0.2f;
        public float TeleportDuration_sec { get; private set; } = 0.1f;

        public void Teleportate(Vertex to, Action onComplete)
        {
            OnStartTeleportation(() =>
            {
                transform.DOMove(to.transform.position, TeleportDuration_sec)
                    .OnComplete(() =>
                    {
                        OnCompleteTeleportation();
                        onComplete?.Invoke();

                        // correct position when teleportate to moving object
                        transform.position = to.transform.position;
                    });
            });
        }

        private void OnStartTeleportation(TweenCallback onComplete)
        {
            transform.DOScale(0f, TeleportDisappearDuration_sec)
                .OnComplete(onComplete);
        }

        private void OnCompleteTeleportation()
        {
            transform.DOScale(1f, TeleportDisappearDuration_sec);
        }
    }
}