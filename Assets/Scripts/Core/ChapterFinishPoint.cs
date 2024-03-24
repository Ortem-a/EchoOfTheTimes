using EchoOfTheTimes.Interfaces;
using System;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class ChapterFinishPoint : MonoBehaviour, ISpecialVertex
    {
        public Action OnEnter {  get; private set; }

        public Action OnExit { get; private set; }


        private void Awake()
        {
            OnEnter += Enter;
            OnExit += Exit;
        }

        private void OnDestroy()
        {
            OnEnter -= Enter;
            OnExit -= Exit;
        }

        private void Enter()
        {
        }

        private void Exit()
        {
        }

        public void Initialize()
        {
        }
    }
}