using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.Persistence;
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

#warning результаты сраной реализации кнопок
            var buttons = FindObjectsOfType<LevelStateButton>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Initialize();
            }

            //SaveLoadSystem.Instance.BindPlayer();
        }
    }
}