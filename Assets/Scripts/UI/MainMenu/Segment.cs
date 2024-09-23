using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(BoxCollider))]
    public class Segment : MonoBehaviour
    {
        private BoxCollider _collider;

        public GameLevel Level;

        private SceneLoader _sceneLoader;

        private void Awake()
        {
            _sceneLoader = FindObjectOfType<SceneLoader>();
            _collider = GetComponent<BoxCollider>();

            SetEnable(false);
        }

        public void SetEnable(bool isEnable)
        {
            _collider.enabled = isEnable;
        }

        public async void HandleTouch()
        {
            Debug.Log($"[{name}] TOUCHED | Last Loaded Level: {Level}");

            if (Level.LevelStatus != StatusType.Locked)
            {
                PersistenceService.LastLoadedLevel = Level;

                await _sceneLoader.LoadSceneGroupAsync(Level);
            }
        }

        public void MarkAs(StatusType status)
        {
            switch (status)
            {
                case StatusType.Locked:
                    MarkAsLocked();
                    break;
                case StatusType.Unlocked:
                    MarkAsUnlocked();
                    break;
                    case StatusType.Completed:
                    MarkAsCompleted();
                    break;
                default:
                    throw new System.NotImplementedException($"Not implemented status '{status}'!");
            }
        }

        private void MarkAsLocked()
        {
            //Debug.Log($"Mark {name} as Locked!");

            GetComponent<Renderer>().material.color = Color.red;
        }

        private void MarkAsUnlocked()
        {
            //Debug.Log($"Mark {name} as Unlocked!");

            GetComponent<Renderer>().material.color = Color.yellow;
        }

        private void MarkAsCompleted()
        {
            //Debug.Log($"Mark {name} as Completed!");

            GetComponent<Renderer>().material.color = Color.green;
        }
    }
}