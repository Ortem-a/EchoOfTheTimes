using EchoOfTheTimes.Core;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class PlayerPath : MonoBehaviour
    {
        public List<Vertex> Path;

        private bool _isMarkNeeded = false;

        public void SetPath(List<Vertex> path)
        {
            Path = path;

            _isMarkNeeded=true;
        }

        private void OnDrawGizmos()
        {
            if (_isMarkNeeded)
            {
                Gizmos.color = Color.magenta;    

                for (int i = 0; i < Path.Count; i++) 
                {
                    Gizmos.DrawCube(Path[i].transform.position + Vector3.up / 2f, Vector3.one * 0.25f);
                }

                Gizmos.color = Color.white;    
            }
        }
    }
}