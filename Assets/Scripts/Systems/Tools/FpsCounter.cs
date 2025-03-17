using UnityEngine;

namespace Systems.Tools
{
    public class FpsCounter : MonoBehaviour
    {
        public int Fps { get; private set; }

        private readonly float _hudRefreshRate = 1f;
        private float _timer;

        private void Update()
        {
            if (Time.unscaledTime > _timer)
            {
                Fps = (int)(1f / Time.unscaledDeltaTime);

                _timer = Time.unscaledTime + _hudRefreshRate;
            }
        }
    }
}