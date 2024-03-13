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
            GameManager.Instance.StateMachine.OnTransitionStart += GameManager.Instance.CommandManager.ForceStop;
            GameManager.Instance.StateMachine.OnTransitionStart += GameManager.Instance.VertexFollower.LinkDefault;
            GameManager.Instance.StateMachine.OnTransitionStart += GameManager.Instance.Graph.ResetVertices;

            GameManager.Instance.StateMachine.OnTransitionComplete += GameManager.Instance.Graph.Load;
            GameManager.Instance.StateMachine.OnTransitionComplete += GameManager.Instance.VertexFollower.Unlink;

            GameManager.Instance.Player.Initialize();
            GameManager.Instance.UserInputHandler.Initialize();
            GameManager.Instance.UserInput.Initialize();
        }

        private void OnDestroy()
        {
            GameManager.Instance.StateMachine.OnTransitionStart -= GameManager.Instance.CommandManager.ForceStop;
            GameManager.Instance.StateMachine.OnTransitionStart -= GameManager.Instance.VertexFollower.LinkDefault;
            GameManager.Instance.StateMachine.OnTransitionStart -= GameManager.Instance.Graph.ResetVertices;

            GameManager.Instance.StateMachine.OnTransitionComplete -= GameManager.Instance.Graph.Load;
            GameManager.Instance.StateMachine.OnTransitionComplete -= GameManager.Instance.VertexFollower.Unlink;
        }
    }
}