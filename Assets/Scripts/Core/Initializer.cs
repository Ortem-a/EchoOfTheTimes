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
            LinksContainer.Instance.StateMachine.OnTransitionStart += LinksContainer.Instance.VertexFollower.LinkDefault;
            LinksContainer.Instance.StateMachine.OnTransitionStart += LinksContainer.Instance.Graph.ResetVertices;
            LinksContainer.Instance.StateMachine.OnTransitionStart += LinksContainer.Instance.CommandManager.ForceStop;

            LinksContainer.Instance.StateMachine.OnTransitionComplete += LinksContainer.Instance.Graph.Load;
            LinksContainer.Instance.StateMachine.OnTransitionComplete += LinksContainer.Instance.VertexFollower.Unlink;

            LinksContainer.Instance.Player.Initialize();
            LinksContainer.Instance.UserInputHandler.Initialize();
        }

        private void OnDestroy()
        {
            LinksContainer.Instance.StateMachine.OnTransitionStart -= LinksContainer.Instance.Graph.ResetVertices;
            LinksContainer.Instance.StateMachine.OnTransitionStart -= LinksContainer.Instance.VertexFollower.LinkDefault;
            LinksContainer.Instance.StateMachine.OnTransitionStart -= LinksContainer.Instance.CommandManager.ForceStop;

            LinksContainer.Instance.StateMachine.OnTransitionComplete -= LinksContainer.Instance.Graph.Load;
            LinksContainer.Instance.StateMachine.OnTransitionComplete -= LinksContainer.Instance.VertexFollower.Unlink;
        }
    }
}