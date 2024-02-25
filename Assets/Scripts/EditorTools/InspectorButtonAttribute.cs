using UnityEngine;

namespace EchoOfTheTimes.EditorTools
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