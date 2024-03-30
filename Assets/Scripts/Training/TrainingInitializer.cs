using EchoOfTheTimes.Core;
using UnityEngine;

namespace EchoOfTheTimes.Training
{
    public class TrainingInitializer : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.UserInputHandler.gameObject.SetActive(false);
        }
    }
}