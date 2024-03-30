#if UNITY_EDITOR
using UnityEngine;

namespace EchoOfTheTimes.Editor
{
    public class InspectorButtonAttribute : PropertyAttribute
    {
        public string MethodName { get; private set; }

        public InspectorButtonAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}
#endif