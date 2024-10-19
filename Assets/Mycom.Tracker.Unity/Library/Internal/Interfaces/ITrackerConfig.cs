using System;

namespace Mycom.Tracker.Unity.Internal.Interfaces
{
    internal interface ITrackerConfig : IDisposable
    {
        String GetId();

        Int32 GetBufferingPeriod();
        void SetBufferingPeriod(Int32 value);

        Int32 GetForcingPeriod();
        void SetForcingPeriod(Int32 value);

        Int32 GetLaunchTimeout();
        void SetLaunchTimeout(Int32 value);

        Boolean IsTrackingEnvironmentEnabled();
        void SetTrackingEnvironmentEnabled(Boolean value);

        Boolean IsTrackingLaunchEnabled();
        void SetTrackingLaunchEnabled(Boolean value);

        Boolean IsTrackingLocationEnabled();
        void SetTrackingLocationEnabled(Boolean value);

        void SetProxyHost(String value);

        Boolean IsRegisterForSkAdAttribution();
        void SetRegisterForSkAdAttribution(Boolean value);
    }
}