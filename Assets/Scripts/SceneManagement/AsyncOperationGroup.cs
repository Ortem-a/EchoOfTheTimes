using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EchoOfTheTimes.SceneManagement
{
    public readonly struct AsyncOperationGroup
    {
        public readonly List<AsyncOperation> Operations;

        public float Progress => Operations.Count == 0 ? 0 : Operations.Average(x => x.progress);
        public bool IsDone => Operations.All(x => x.isDone);

        public AsyncOperationGroup(int initialCapacity)
        {
            Operations = new List<AsyncOperation>(initialCapacity);
        }
    }
}