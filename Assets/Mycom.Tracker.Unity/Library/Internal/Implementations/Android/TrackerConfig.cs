#if UNITY_ANDROID
using System;
using Mycom.Tracker.Unity.Internal.Interfaces;
using UnityEngine;

namespace Mycom.Tracker.Unity.Internal.Implementations.Android
{
    internal sealed class TrackerConfig: ITrackerConfig
    {
        private readonly AndroidJavaObject _trackerConfigObject;
        private Boolean _isDisposed;

        internal TrackerConfig(AndroidJavaObject trackerConfigObject)
        {
            _trackerConfigObject = trackerConfigObject;
        }

        ~TrackerConfig()
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

            if (_trackerConfigObject != null)
            {
                _trackerConfigObject.Dispose();
            }
        }

        String ITrackerConfig.GetId()
        {
            return _trackerConfigObject.GetString("getId");
        }

        Int32 ITrackerConfig.GetBufferingPeriod()
        {
            return _trackerConfigObject.Call<Int32>("getBufferingPeriod");
        }

        void ITrackerConfig.SetBufferingPeriod(Int32 value)
        {
            _trackerConfigObject.Call<AndroidJavaObject>("setBufferingPeriod", value);
        }

        int ITrackerConfig.GetForcingPeriod()
        {
            return _trackerConfigObject.Call<Int32>("getForcingPeriod");
        }

        void ITrackerConfig.SetForcingPeriod(Int32 value)
        {
            _trackerConfigObject.Call<AndroidJavaObject>("setForcingPeriod", value);
        }

        Int32 ITrackerConfig.GetLaunchTimeout()
        {
            return _trackerConfigObject.Call<Int32>("getLaunchTimeout");
        }

        void ITrackerConfig.SetLaunchTimeout(Int32 value)
        {
            _trackerConfigObject.Call<AndroidJavaObject>("setLaunchTimeout", value);
        }

        Boolean ITrackerConfig.IsTrackingEnvironmentEnabled()
        {
            return _trackerConfigObject.Call<Boolean>("isTrackingEnvironmentEnabled");
        }

        Boolean ITrackerConfig.IsRegisterForSkAdAttribution()
        {
            return false;
        }

        void ITrackerConfig.SetTrackingEnvironmentEnabled(Boolean value)
        {
            _trackerConfigObject.Call<AndroidJavaObject>("setTrackingEnvironmentEnabled", value);
        }

        Boolean ITrackerConfig.IsTrackingLaunchEnabled()
        {
            return _trackerConfigObject.Call<Boolean>("isTrackingLaunchEnabled");
        }

        void ITrackerConfig.SetTrackingLaunchEnabled(Boolean value)
        {
            _trackerConfigObject.Call<AndroidJavaObject>("setTrackingLaunchEnabled", value);
        }

        Boolean ITrackerConfig.IsTrackingLocationEnabled()
        {
            return _trackerConfigObject.Call<Boolean>("isTrackingLocationEnabled");
        }

        void ITrackerConfig.SetTrackingLocationEnabled(Boolean value)
        {
            _trackerConfigObject.Call<AndroidJavaObject>("setTrackingLocationEnabled", value);
        }

        void ITrackerConfig.SetProxyHost(string value)
        {
            _trackerConfigObject.Call<AndroidJavaObject>("setProxyHost", value);
        }

        void ITrackerConfig.SetRegisterForSkAdAttribution(Boolean value)
        { }
    }
}

#endif