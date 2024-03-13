using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class RefinedOrbitCamera : MonoBehaviour
    {
        public Transform centralAxis; // Central axis to orbit around.
        public Transform player; // The player, which the camera keeps in view when not manually rotating.
        public float orbitDistance = 10f; // Distance of the camera from the central axis.
        public float orbitHeight = 5f; // Height of the camera orbit relative to the central axis.
        public float followSpeed = 5f; // Speed at which the camera adjusts its orbit.

        private void LateUpdate()
        {
            // Calculate direction from the axis to the player
            Vector3 axisToPlayerDirection = (player.position - centralAxis.position).normalized;

            // Position the camera behind the player, maintaining the specified orbit distance and height
            Vector3 cameraPositionBehindPlayer = player.position + axisToPlayerDirection * orbitDistance;
            cameraPositionBehindPlayer.y += orbitHeight; // Adjust height

            // Smoothly move the camera to the new position
            transform.position = Vector3.Lerp(transform.position, cameraPositionBehindPlayer, Time.deltaTime * followSpeed);

            // Look at the axis through the player
            transform.LookAt(centralAxis.position);
        }
    }
}