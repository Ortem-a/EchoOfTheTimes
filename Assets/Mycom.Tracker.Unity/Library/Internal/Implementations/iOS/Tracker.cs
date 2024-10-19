#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AOT;
using Mycom.Tracker.Unity.Ads;
using Mycom.Tracker.Unity.Internal.Interfaces;

namespace Mycom.Tracker.Unity.Internal.Implementations.iOS
{
    internal sealed class Tracker : ITracker
    {
        internal static readonly ITracker Instance = new Tracker();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerGetInstanceId();

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerInitTracker(String id);

        [DllImport("__Internal")]
        private static extern Boolean MRMTMyTrackerIsDebugMode();

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerSetAttributionListener(Action<String> listener);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerSetDebugMode(Boolean value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerTrackEvent(String name, String eventParams);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerTrackInvite(String eventParams);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerTrackLevelLevelParams(Int32 level, String eventParams);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerTrackLevelParams(String eventParams);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerTrackLogin(String userId, String vkConnectId, String eventParams);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerTrackRegistration(String userId, String vkConnectId, String eventParams);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerTrackAdEvent(int appEvent,
                                                             int network,
                                                             double revenue,
                                                             string currency,
                                                             string source,
                                                             string placementId,
                                                             string adId,
                                                             string format);


        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerFlush();

        [MonoPInvokeCallback(typeof(Action<String>))]
        private static void OnAttributionReceived(String deepLink)
        {
            var listener = ((Tracker)Instance)._attributionListener;
            if (listener == null)
            {
                return;
            }

            if (deepLink == null)
            {
                return;
            }

            listener(new MyTrackerAttribution(deepLink));
        }

        private static String SerializeToString(IDictionary<String, String> dictionary)
        {
            return dictionary == null ? null : "{" + String.Join(",", dictionary.Select(pair => String.Format("\"{0}\":\"{1}\"", pair.Key, pair.Value)).ToArray()) + "}";
        }

        private readonly MyTrackerParams _myTrackerParams;
        private readonly MyTrackerConfig _myTrackerConfig;

        private volatile Action<MyTrackerAttribution> _attributionListener;

        private Tracker()
        {
            _myTrackerParams = new MyTrackerParams(new TrackerParams());
            _myTrackerConfig = new MyTrackerConfig(new TrackerConfig());
        }

        void IDisposable.Dispose() { }

        String ITracker.GetInstanceId()
        {
            return MRMTMyTrackerGetInstanceId();
        }

        void ITracker.Init(string id)
        {
            MRMTMyTrackerInitTracker(id);
        }

        Boolean ITracker.IsDebugMode()
        {
            return MRMTMyTrackerIsDebugMode();
        }

        void ITracker.SetAttributionListener(Action<MyTrackerAttribution> listener)
        {
            lock (this)
            {
                _attributionListener = listener;
                MRMTMyTrackerSetAttributionListener(_attributionListener == null ? default(Action<String>) : OnAttributionReceived);
            }
        }

        void ITracker.SetDebugMode(Boolean value)
        {
            MRMTMyTrackerSetDebugMode(value);
        }

        void ITracker.TrackEvent(String name, IDictionary<String, String> eventParams)
        {
            MRMTMyTrackerTrackEvent(name, SerializeToString(eventParams));
        }

        void ITracker.TrackInviteEvent(IDictionary<String, String> eventParams)
        {
            MRMTMyTrackerTrackInvite(SerializeToString(eventParams));
        }

        void ITracker.TrackLevelEvent(Int32? level, IDictionary<String, String> eventParams)
        {
            var paramsString = SerializeToString(eventParams);
            if (level.HasValue)
            {
                MRMTMyTrackerTrackLevelLevelParams(level.Value, paramsString);
            }
            else
            {
                MRMTMyTrackerTrackLevelParams(paramsString);
            }
        }

        void ITracker.TrackLoginEvent(String userId, String vkConnectId, IDictionary<String, String> eventParams)
        {
            MRMTMyTrackerTrackLogin(userId, vkConnectId, SerializeToString(eventParams));
        }

        void ITracker.TrackRegistrationEvent(String userId, String vkConnectId, IDictionary<String, String> eventParams)
        {
            MRMTMyTrackerTrackRegistration(userId, vkConnectId, SerializeToString(eventParams));
        }

        void ITracker.TrackAdEvent(AdEvent adEvent)
        {
            MRMTMyTrackerTrackAdEvent((int)adEvent.appEvent,
                                      (int)adEvent.network,
                                      adEvent.revenue,
                                      adEvent.currency,
                                      adEvent.source,
                                      adEvent.placementId,
                                      adEvent.adId,
                                      adEvent.adFormat);
        }

        void ITracker.Flush()
        {
            MRMTMyTrackerFlush();
        }

        MyTrackerParams ITracker.MyTrackerParams
        {
            get { return _myTrackerParams; }
        }

        MyTrackerConfig ITracker.MyTrackerConfig
        {
            get { return _myTrackerConfig; }
        }

#if UNITY_PURCHASING
        void ITracker.TrackPurchaseEvent(UnityEngine.Purchasing.Product product, IDictionary<String, String> eventParams) { }
#endif
    }
}

#endif