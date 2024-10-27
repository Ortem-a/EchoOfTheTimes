using EchoOfTheTimes.Interfaces;
using UnityEngine;

namespace EchoOfTheTimes.Collectables
{
    [RequireComponent(typeof(CapsuleCollider), typeof(CollectableEffectsService))]
    public class Collectable : MonoBehaviour, ICollectable
    {
        private CapsuleCollider _collider;
        private CollectableEffectsService _effectsService;
        private CollectableService _service;

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
            _collider.isTrigger = true;

            _effectsService = GetComponent<CollectableEffectsService>();
            _effectsService.Idle();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Collect();
                _effectsService.OnCollect(() => Destroy(gameObject));
            }
        }

        public void Initialize(CollectableService service) => _service = service;

        public void Collect() => _service.OnCollected();
    }
}