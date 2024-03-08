using EchoOfTheTimes.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Persistence
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize<T>(T obj)
        {
            return JsonUtility.ToJson(obj, prettyPrint: true);
        }

        public T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}