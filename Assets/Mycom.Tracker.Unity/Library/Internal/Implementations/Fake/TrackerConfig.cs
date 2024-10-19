using Mycom.Tracker.Unity.Internal.Interfaces;

namespace Mycom.Tracker.Unity.Internal.Implementations.Fake
{
    internal sealed class TrackerConfig : ITrackerConfig
    {
        public void Dispose() { }

        public int GetBufferingPeriod() { return 0; }

        public int GetForcingPeriod() { return 0; }

        public string GetId() { return null; }

        public int GetLaunchTimeout() { return 0; }

        public bool IsTrackingEnvironmentEnabled() { return false; }

        public bool IsTrackingLaunchEnabled() { return false; }

        public bool IsTrackingLocationEnabled() { return false; }

        public bool IsRegisterForSkAdAttribution() { return true; }

        public void SetBufferingPeriod(int value) { }

        public void SetForcingPeriod(int value) { }

        public void SetLaunchTimeout(int value) { }

        public void SetProxyHost(string value) { }

        public void SetTrackingEnvironmentEnabled(bool value) { }

        public void SetTrackingLaunchEnabled(bool value) { }

        public void SetTrackingLocationEnabled(bool value) { }

        public void SetRegisterForSkAdAttribution(bool value) { }
    }
}