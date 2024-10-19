using System;
using System.Collections.Generic;
using Mycom.Tracker.Unity.Ads;
using Mycom.Tracker.Unity.Internal.Interfaces;

namespace Mycom.Tracker.Unity.Internal.Implementations.Fake
{
    internal sealed class Tracker : ITracker
    {
        public static ITracker Instance = new Tracker();

        MyTrackerParams ITracker.MyTrackerParams
        {
            get
            {
                return new MyTrackerParams(new TrackerParams());
            }
        }

        MyTrackerConfig ITracker.MyTrackerConfig
        {
            get
            {
                return new MyTrackerConfig(new TrackerConfig());
            }
        }

        void IDisposable.Dispose() { }

        void ITracker.Flush() { }

        void ITracker.Init(string id) { }

        Boolean ITracker.IsDebugMode()
        {
            return false;
        }

        public bool IsEnabled()
        {
            return false;
        }

        void ITracker.SetAttributionListener(Action<MyTrackerAttribution> listener) { }

        void ITracker.SetDebugMode(bool value) { }

        public void SetEnabled(bool value) { }

        void ITracker.TrackEvent(string name, IDictionary<string, string> eventParams) { }

        void ITracker.TrackInviteEvent(IDictionary<string, string> eventParams) { }

        void ITracker.TrackLevelEvent(int? level, IDictionary<string, string> eventParams) { }

        void ITracker.TrackLoginEvent(String userId, String vkConnectId, IDictionary<string, string> eventParams) { }

        void ITracker.TrackRegistrationEvent(String userId, String vkConnectId, IDictionary<string, string> eventParams) { }

        void ITracker.TrackAdEvent(AdEvent adEvent) { }

        string ITracker.GetInstanceId()
        {
            return null;
        }

#if UNITY_ANDROID
        void ITracker.TrackPurchaseEvent(String skuDetails, String purchaseData, String dataSignature, IDictionary<String, String> eventParams) { }
#endif

#if UNITY_PURCHASING
        void ITracker.TrackPurchaseEvent(UnityEngine.Purchasing.Product product, IDictionary<String, String> eventParams) { }
#endif
    }
}