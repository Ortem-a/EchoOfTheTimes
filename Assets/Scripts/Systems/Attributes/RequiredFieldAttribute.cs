using System;
using UnityEngine;

namespace Systems
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredFieldAttribute : PropertyAttribute { }
}