#if UNITY_IOS
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Mycom.Tracker.Unity.Internal.Interfaces;
using UnityEngine;

namespace Mycom.Tracker.Unity.Internal.Implementations.iOS
{
    internal sealed class TrackerConfig : ITrackerConfig
    {
        [DllImport("__Internal")]
        private static extern Int32 MRMTMyTrackerConfigGetBufferingPeriod();

        [DllImport("__Internal")]
        private static extern Int32 MRMTMyTrackerConfigGetForcingPeriod();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerConfigGetId();

        [DllImport("__Internal")]
        private static extern Int32 MRMTMyTrackerConfigGetLaunchTimeout();

        [DllImport("__Internal")]
        private static extern Boolean MRMTMyTrackerConfigIsTrackingEnvironmentEnabled();

        [DllImport("__Internal")]
        private static extern Boolean MRMTMyTrackerConfigIsTrackingLaunchEnabled();

        [DllImport("__Internal")]
        private static extern Boolean MRMTMyTrackerConfigIsTrackingLocationEnabled();

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerConfigSetBufferingPeriod(Int32 value);

        [DllImport("__Internal")]
        private static extern Int32 MRMTMyTrackerConfigSetForcingPeriod(Int32 value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerConfigSetLaunchTimeout(Int32 value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerConfigSetTrackingEnvironmentEnabled(Boolean value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerConfigSetTrackingLaunchEnabled(Boolean value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerConfigSetTrackingLocationEnabled(Boolean value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerConfigSetProxyHost(String value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerConfigSetRegisterForSkAdAttribution(Boolean value);

        [DllImport("__Internal")]
        private static extern Boolean MRMTMyTrackerConfigIsRegisterForSkAdAttribution();

        void IDisposable.Dispose() { }

        Int32 ITrackerConfig.GetBufferingPeriod()
        {
            return MRMTMyTrackerConfigGetBufferingPeriod();
        }

        int ITrackerConfig.GetForcingPeriod()
        {
            return MRMTMyTrackerConfigGetForcingPeriod();
        }

        String ITrackerConfig.GetId()
        {
            return MRMTMyTrackerConfigGetId();
        }

        Int32 ITrackerConfig.GetLaunchTimeout()
        {
            return MRMTMyTrackerConfigGetLaunchTimeout();
        }

        Boolean ITrackerConfig.IsTrackingEnvironmentEnabled()
        {
            return MRMTMyTrackerConfigIsTrackingEnvironmentEnabled();
        }

        Boolean ITrackerConfig.IsTrackingLaunchEnabled()
        {
            return MRMTMyTrackerConfigIsTrackingLaunchEnabled();
        }

        Boolean ITrackerConfig.IsTrackingLocationEnabled()
        {
            return MRMTMyTrackerConfigIsTrackingLocationEnabled();
        }

        Boolean ITrackerConfig.IsRegisterForSkAdAttribution()
        {
            return MRMTMyTrackerConfigIsRegisterForSkAdAttribution();
        }

        void ITrackerConfig.SetBufferingPeriod(Int32 value)
        {
            MRMTMyTrackerConfigSetBufferingPeriod(value);
        }

        void ITrackerConfig.SetForcingPeriod(Int32 value)
        {
            MRMTMyTrackerConfigSetForcingPeriod(value);
        }

        void ITrackerConfig.SetLaunchTimeout(Int32 value)
        {
            MRMTMyTrackerConfigSetLaunchTimeout(value);
        }

        void ITrackerConfig.SetTrackingEnvironmentEnabled(Boolean value)
        {
            MRMTMyTrackerConfigSetTrackingEnvironmentEnabled(value);
        }

        void ITrackerConfig.SetTrackingLaunchEnabled(Boolean value)
        {
            MRMTMyTrackerConfigSetTrackingLaunchEnabled(value);
        }

        void ITrackerConfig.SetTrackingLocationEnabled(Boolean value)
        {
            MRMTMyTrackerConfigSetTrackingLocationEnabled(value);
        }

        void ITrackerConfig.SetProxyHost(String value)
        {
            MRMTMyTrackerConfigSetProxyHost(value);
        }

        void ITrackerConfig.SetRegisterForSkAdAttribution(Boolean value)
        {
            MRMTMyTrackerConfigSetRegisterForSkAdAttribution(value);
        }
    }
}

#endif