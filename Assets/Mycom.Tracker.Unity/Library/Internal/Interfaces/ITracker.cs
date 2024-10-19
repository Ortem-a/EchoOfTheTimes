using System;
using System.Collections.Generic;
using Mycom.Tracker.Unity.Ads;

#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

namespace Mycom.Tracker.Unity.Internal.Interfaces
{
    internal interface ITracker : IDisposable
    {
        MyTrackerParams MyTrackerParams { get; }

        MyTrackerConfig MyTrackerConfig { get; }

        String GetInstanceId();

        void Init(String id);

        Boolean IsDebugMode();

        void SetAttributionListener(Action<MyTrackerAttribution> listener);

        void SetDebugMode(Boolean value);

        void TrackEvent(String name, IDictionary<String, String> eventParams = null);

        void TrackInviteEvent(IDictionary<String, String> eventParams = null);

        void TrackLevelEvent(Int32? level = null, IDictionary<String, String> eventParams = null);

        void TrackLoginEvent(String userId, String vkConnectId, IDictionary<String, String> eventParams = null);

        void TrackRegistrationEvent(String userId, String vkConnectId, IDictionary<String, String> eventParams = null);

        void TrackAdEvent(AdEvent adEvent);

        void Flush();

#if UNITY_ANDROID
        void TrackPurchaseEvent(String skuDetails, String purchaseData, String dataSignature, IDictionary<String, String> eventParams = null);
#endif

#if UNITY_PURCHASING
        void TrackPurchaseEvent(Product product, IDictionary<String, String> eventParams = null);
#endif
    }
}