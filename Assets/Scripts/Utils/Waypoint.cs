using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class Waypoint : MonoBehaviour
    {
        private float _gizmoRadius = 0.1f;

        [SerializeField]
        private Vector3 _offset;

        public BezierCurve Curve;

        public Vector3 Point => transform.position + _offset;

        //private void OnValidate()
        //{
        //    Curve = Curve != null ? Curve : GetComponentInChildren<BezierCurve>();
        //    Curve.transform.position = Point;
        //}

        private void Awake()
        {
            Curve = GetComponentInChildren<BezierCurve>();
            Curve.transform.position = Point;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(Point, _gizmoRadius);

            Gizmos.color = Color.white;
        }
    }
}
