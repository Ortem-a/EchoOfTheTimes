using EchoOfTheTimes.Core;
using System.Collections;
using System.Collections.Generic;
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