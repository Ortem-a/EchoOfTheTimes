using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        if (_camera == null)
        {
            Debug.LogError("Main Camera not found!");
        }
    }

    public void ShakeCamera(float intensity, float frequency, float duration, float falloff, float randomness, bool shakeOnX, bool shakeOnY, float delay)
    {
        StartCoroutine(Shake(intensity, frequency, duration, falloff, randomness, shakeOnX, shakeOnY, delay));
    }

    private IEnumerator Shake(float intensity, float frequency, float duration, float falloff, float randomness, bool shakeOnX, bool shakeOnY, float delay)
    {
        if (_camera == null)
        {
            Debug.LogError("Camera not assigned!");
            yield break;
        }

        yield return new WaitForSeconds(delay);

        Vector3 originalPosition = _camera.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = shakeOnX ? Random.Range(-1f, 1f) * intensity * (1 - elapsed / duration) * randomness : 0;
            float y = shakeOnY ? Random.Range(-1f, 1f) * intensity * (1 - elapsed / duration) * randomness : 0;

            _camera.transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _camera.transform.localPosition = originalPosition;
    }
}
