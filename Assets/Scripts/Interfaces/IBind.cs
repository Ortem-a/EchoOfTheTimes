using EchoOfTheTimes.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Interfaces
{
    public interface IBind<TData> where TData : ISaveable
    {
        SerializableGuid Id { get; set; }
        void Bind(TData data);
    }
}