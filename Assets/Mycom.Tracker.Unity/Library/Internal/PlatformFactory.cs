using System;
using Mycom.Tracker.Unity.Internal.Interfaces;

namespace Mycom.Tracker.Unity.Internal
{
    internal static class PlatformFactory
    {
        internal static ITracker CreateTracker()
        {
#if UNITY_ANDROID
            return Implementations.Android.Tracker.Instance;
#elif UNITY_IOS
            return Implementations.iOS.Tracker.Instance;
#else
            return Implementations.Fake.Tracker.Instance;
#endif
        }

        internal static String GetInstanceId()
        {
#if UNITY_ANDROID
            return Implementations.Android.Tracker.Instance.GetInstanceId();
#elif UNITY_IOS
            return Implementations.iOS.Tracker.Instance.GetInstanceId();
#else
            return null;
#endif

        }
    }
}