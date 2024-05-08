using EchoOfTheTimes.Units;
using UnityEngine;

namespace EchoOfTheTimes.Training
{
    [RequireComponent(typeof(SphereCollider))]
    public class TrainerDisabler : MonoBehaviour
    {
        public MechanicTrainer Trainer;

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                Trainer.Disable();
            }
        }
    }
}