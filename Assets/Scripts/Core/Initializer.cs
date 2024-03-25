using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.LevelStates;
using System.Linq;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class Initializer : MonoBehaviour
    {
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            GameManager.Instance.VertexFollower.Initialize();
            GameManager.Instance.Player.Initialize();
            GameManager.Instance.UserInputHandler.Initialize();
            GameManager.Instance.UserInput.Initialize();
            GameManager.Instance.CheckpointManager.Initialize();

            var specialVertices = FindObjectsOfType<MonoBehaviour>().OfType<ISpecialVertex>().ToArray();
            for (int i = 0; i < specialVertices.Length; i++)
            {
                specialVertices[i].Initialize();
            }

            //SaveLoadSystem.Instance.BindPlayer();
        }
    }
}