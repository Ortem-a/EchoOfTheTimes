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
            GameManager.Instance.Player.Initialize();
            GameManager.Instance.UserInputHandler.Initialize();
            GameManager.Instance.UserInput.Initialize();

            SaveLoadSystem.Instance.BindPlayer();
        }
    }
}