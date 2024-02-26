using EchoOfTheTimes.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class ClickMover : AbstractUnit
    {
        [SerializeField]
        private Camera _camera;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                Vector3 worldPosition = ScreenToWorldPosition(Input.mousePosition);

                MoveTo(worldPosition);
            }
        }

        public Vector3 ScreenToWorldPosition(Vector3 screenPosition)
        {
            Vector3 worldPosition = Vector3.zero;
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hitData, 1000f))
            {
                worldPosition = hitData.point;
            }

            return worldPosition;
        }
    }
}