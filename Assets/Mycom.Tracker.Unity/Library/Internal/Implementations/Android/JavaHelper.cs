#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Mycom.Tracker.Unity.Internal.Implementations.Android
{
    internal static class JavaHelper
    {
        internal static AndroidJavaObject CreateJavaJsonObbject(IDictionary<String, String> value)
        {
            if (value == null)
            {
                return null;
            }

            using (var map = CreateJavaStringMap(value))
            {
                return new AndroidJavaObject("org.json.JSONObject", map);
            }
        }

        internal static AndroidJavaObject CreateJavaJsonObbject(String value)
        {
            using (var javaString = CreateJavaString(value))
            {
                return value == null ? null : new AndroidJavaObject("org.json.JSONObject", javaString);
            }
        }

        internal static AndroidJavaObject CreateJavaString(String value)
        {
            return value == null ? null : new AndroidJavaObject("java.lang.String", value);
        }

        internal static AndroidJavaObject CreateJavaStringArray(IList<String> values)
        {
            if (values == null)
            {
                return null;
            }

            using (var arrayClass = new AndroidJavaClass("java.lang.reflect.Array"))
            {
                using (var stringClass = new AndroidJavaClass("java.lang.String"))
                {
                    var arrayObject = arrayClass.CallStatic<AndroidJavaObject>("newInstance", stringClass, values.Count);
                    for (var i = 0; i < values.Count; ++i)
                    {
                        using (var stringValue = new AndroidJavaObject("java.lang.String", values[i]))
                        {
                            arrayClass.CallStatic("set", arrayObject, i, stringValue);
                        }
                    }
                    return arrayObject;
                }
            }
        }

        internal static AndroidJavaObject CreateJavaStringMap(IDictionary<String, String> value)
        {
            if (value == null)
            {
                return null;
            }

            var hashMap = new AndroidJavaObject("java.util.HashMap", value.Count);
            var methodId = AndroidJNIHelper.GetMethodID(hashMap.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
            foreach (var pair in value)
            {
                using (var keyObject = CreateJavaString(pair.Key))
                {
                    using (var valueObject = CreateJavaString(pair.Value))
                    {
                        AndroidJNI.CallObjectMethod(hashMap.GetRawObject(),
                                                    methodId,
                                                    AndroidJNIHelper.CreateJNIArgArray(new Object[] { keyObject, valueObject }));
                    }
                }
            }
            return hashMap;
        }

        internal static String[] CreateStringArray(AndroidJavaObject javaArray)
        {
            if (javaArray == null)
            {
                return null;
            }

            var rawObject = javaArray.GetRawObject();
            return rawObject.ToInt32() != 0 ? AndroidJNIHelper.ConvertFromJNIArray<String[]>(rawObject) : null;
        }

        internal static String GetString(this AndroidJavaObject javaObject, string methodName, params object[] args)
        {
#if UNITY_2018_2_OR_NEWER
            var tempObject = javaObject.Call<AndroidJavaObject>(methodName, args);
            if (tempObject == null)
            {
                return null;
            }
#endif

            return javaObject.Call<String>(methodName, args);
        }
    }
}

#endif