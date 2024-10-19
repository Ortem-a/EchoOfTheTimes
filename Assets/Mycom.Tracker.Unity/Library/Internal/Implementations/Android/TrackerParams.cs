#if UNITY_ANDROID
using System;
using Mycom.Tracker.Unity.Internal.Interfaces;
using UnityEngine;

namespace Mycom.Tracker.Unity.Internal.Implementations.Android
{
    internal sealed class TrackerParams : ITrackerParams
    {
        private readonly AndroidJavaObject _trackerParamsObject;
        private Boolean _isDisposed;

        internal TrackerParams(AndroidJavaObject trackerParamsObject)
        {
            _trackerParamsObject = trackerParamsObject;
        }

        ~TrackerParams()
        {
            ((IDisposable)this).Dispose();
        }

        void IDisposable.Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;

            if (_trackerParamsObject != null)
            {
                _trackerParamsObject.Dispose();
            }
        }

        Int32 ITrackerParams.GetAge()
        {
            return _trackerParamsObject.Call<Int32>("getAge");
        }

        String[] ITrackerParams.GetCustomUserIds()
        {
            using (var androidJavaObject = _trackerParamsObject.Call<AndroidJavaObject>("getCustomUserIds"))
            {
                return JavaHelper.CreateStringArray(androidJavaObject);
            }
        }

        String[] ITrackerParams.GetEmails()
        {
            using (var androidJavaObject = _trackerParamsObject.Call<AndroidJavaObject>("getEmails"))
            {
                return JavaHelper.CreateStringArray(androidJavaObject);
            }
        }

        GenderEnum ITrackerParams.GetGender()
        {
            return (GenderEnum)_trackerParamsObject.Call<Int32>("getGender");
        }

        String[] ITrackerParams.GetIcqIds()
        {
            using (var androidJavaObject = _trackerParamsObject.Call<AndroidJavaObject>("getIcqIds"))
            {
                return JavaHelper.CreateStringArray(androidJavaObject);
            }
        }

        String ITrackerParams.GetLang()
        {
            return _trackerParamsObject.GetString("getLang");
        }

        String ITrackerParams.GetMrgsAppId()
        {
            return _trackerParamsObject.GetString("getMrgsAppId");
        }

        String ITrackerParams.GetMrgsId()
        {
            return _trackerParamsObject.GetString("getMrgsId");
        }

        String ITrackerParams.GetMrgsUserId()
        {
            return _trackerParamsObject.GetString("getMrgsUserId");
        }

        String[] ITrackerParams.GetOkIds()
        {
            using (var androidJavaObject = _trackerParamsObject.Call<AndroidJavaObject>("getOkIds"))
            {
                return JavaHelper.CreateStringArray(androidJavaObject);
            }
        }

        String[] ITrackerParams.GetPhones()
        {
            using (var androidJavaObject = _trackerParamsObject.Call<AndroidJavaObject>("getPhones"))
            {
                return JavaHelper.CreateStringArray(androidJavaObject);
            }
        }

        String[] ITrackerParams.GetVkIds()
        {
            using (var androidJavaObject = _trackerParamsObject.Call<AndroidJavaObject>("getVkIds"))
            {
                return JavaHelper.CreateStringArray(androidJavaObject);
            }
        }

        String[] ITrackerParams.GetVkConnectIds()
        {
            using (var androidJavaObject = _trackerParamsObject.Call<AndroidJavaObject>("getVkConnectIds"))
            {
                return JavaHelper.CreateStringArray(androidJavaObject);
            }
        }

        void ITrackerParams.SetAge(Int32 value)
        {
            _trackerParamsObject.Call<AndroidJavaObject>("setAge", value);
        }

        void ITrackerParams.SetCustomUserIds(String[] value)
        {
            using (var androidJavaObject = JavaHelper.CreateJavaStringArray(value))
            {
                _trackerParamsObject.Call<AndroidJavaObject>("setCustomUserIds", androidJavaObject);
            }
        }

        void ITrackerParams.SetEmails(String[] value)
        {
            using (var androidJavaObject = JavaHelper.CreateJavaStringArray(value))
            {
                _trackerParamsObject.Call<AndroidJavaObject>("setEmails", androidJavaObject);
            }
        }

        void ITrackerParams.SetGender(GenderEnum value)
        {
            _trackerParamsObject.Call<AndroidJavaObject>("setGender", (Int32)value);
        }

        void ITrackerParams.SetIcqIds(String[] value)
        {
            using (var androidJavaObject = JavaHelper.CreateJavaStringArray(value))
            {
                _trackerParamsObject.Call<AndroidJavaObject>("setIcqIds", androidJavaObject);
            }
        }

        void ITrackerParams.SetLang(String value)
        {
            using (var androidJavaObject = JavaHelper.CreateJavaString(value))
            {
                _trackerParamsObject.Call<AndroidJavaObject>("setLang", androidJavaObject);
            }
        }

        void ITrackerParams.SetMrgsAppId(String value)
        {
            using (var androidJavaObject = JavaHelper.CreateJavaString(value))
            {
                _trackerParamsObject.Call<AndroidJavaObject>("setMrgsAppId", androidJavaObject);
            }
        }

        void ITrackerParams.SetMrgsId(String value)
        {
            using (var androidJavaObject = JavaHelper.CreateJavaString(value))
            {
                _trackerParamsObject.Call<AndroidJavaObject>("setMrgsId", androidJavaObject);
            }
        }

        void ITrackerParams.SetMrgsUserId(String value)
        {
            using (var androidJavaObject = JavaHelper.CreateJavaString(value))
            {
                _trackerParamsObject.Call<AndroidJavaObject>("setMrgsUserId", androidJavaObject);
            }
        }

        void ITrackerParams.SetOkIds(String[] value)
        {
            using (var androidJavaObject = JavaHelper.CreateJavaStringArray(value))
            {
                _trackerParamsObject.Call<AndroidJavaObject>("setOkIds", androidJavaObject);
            }
        }

        void ITrackerParams.SetPhones(String[] value)
        {
            using (var androidJavaObject = JavaHelper.CreateJavaStringArray(value))
            {
                _trackerParamsObject.Call<AndroidJavaObject>("setPhones", androidJavaObject);
            }
        }

        void ITrackerParams.SetVkIds(String[] value)
        {
            using (var androidJavaObject = JavaHelper.CreateJavaStringArray(value))
            {
                _trackerParamsObject.Call<AndroidJavaObject>("setVkIds", androidJavaObject);
            }
        }

        void ITrackerParams.SetVkConnectIds(String[] value)
        {
            using (var androidJavaObject = JavaHelper.CreateJavaStringArray(value))
            {
                _trackerParamsObject.Call<AndroidJavaObject>("setVkConnectIds", androidJavaObject);
            }
        }
    }
}

#endif