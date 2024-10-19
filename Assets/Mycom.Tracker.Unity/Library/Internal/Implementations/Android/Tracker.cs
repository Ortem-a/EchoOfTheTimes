#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using Mycom.Tracker.Unity.Ads;
using Mycom.Tracker.Unity.Internal.Interfaces;
using UnityEngine;

namespace Mycom.Tracker.Unity.Internal.Implementations.Android
{
    internal sealed class Tracker : ITracker
    {
        private const String PriceAmountKey = "price_amount_micros";
        private const String PriceCurrencyCode = "price_currency_code";

        private const Int32 PriceMultiplier = 1000000;

        internal static readonly ITracker Instance = new Tracker();

        private static AndroidJavaObject GetCurrentActivity()
        {
            using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                return player.GetStatic<AndroidJavaObject>("currentActivity");
            }
        }

        private readonly AttributionListenerImpl _attributionListenerImpl;
        private readonly AndroidJavaClass _trackerClass;
        private readonly AndroidJavaClass _adEventBuilderClass;
        private readonly TrackerParams _trackerParams;
        private readonly TrackerConfig _trackerConfig;
        private readonly MyTrackerParams _myTrackerParams;
        private readonly MyTrackerConfig _myTrackerConfig;

        private volatile Action<MyTrackerAttribution> _attributionListener;

        private Boolean _isDisposed;

        private Tracker()
        {
            _trackerClass = new AndroidJavaClass("com.my.tracker.MyTracker");
            _adEventBuilderClass = new AndroidJavaClass("com.my.tracker.ads.AdEventBuilder");

            _attributionListenerImpl = new AttributionListenerImpl(this);

            var javaTrackerParams = _trackerClass.CallStatic<AndroidJavaObject>("getTrackerParams");
            if (javaTrackerParams == null)
            {
                LibraryLogger.Log("Tracker params is null");

                _trackerParams = null;
                _myTrackerParams = null;
            }
            else
            {
                _trackerParams = new TrackerParams(javaTrackerParams);
                _myTrackerParams = new MyTrackerParams(_trackerParams);
            }

            var javaTrackerConfig = _trackerClass.CallStatic<AndroidJavaObject>("getTrackerConfig");
            if (javaTrackerConfig == null)
            {
                LibraryLogger.Log("Tracker config is null");

                _trackerConfig = null;
                _myTrackerConfig = null;
            }
            else
            {
                javaTrackerConfig.Call<AndroidJavaObject>("setAutotrackingPurchaseEnabled", false);

                _trackerConfig = new TrackerConfig(javaTrackerConfig);
                _myTrackerConfig = new MyTrackerConfig(_trackerConfig);
            }
        }

        void IDisposable.Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;

            if (_trackerParams == null)
            {
                ((IDisposable)_trackerParams).Dispose();
            }
            if (_trackerConfig == null)
            {
                ((IDisposable)_trackerConfig).Dispose();
            }

            _trackerClass.Dispose();
        }

        String ITracker.GetInstanceId()
        {
            using (var activity = GetCurrentActivity())
            {
                return _trackerClass.CallStatic<String>("getInstanceId", activity);
            }
        }

        void ITracker.Init(String id)
        {
            using (var activity = GetCurrentActivity())
            {
                using (var application = activity.Call<AndroidJavaObject>("getApplication"))
                {
                    _trackerClass.CallStatic("initTracker", id, application);
                    _trackerClass.CallStatic("trackLaunchManually", activity);
                }
            }
        }

        Boolean ITracker.IsDebugMode()
        {
            return _trackerClass.CallStatic<Boolean>("isDebugMode");
        }

        void ITracker.SetAttributionListener(Action<MyTrackerAttribution> listener)
        {
            lock (this)
            {
                _attributionListener = listener;
                _trackerClass.CallStatic("setAttributionListener", listener != null ? _attributionListenerImpl : null);
            }
        }

        void ITracker.SetDebugMode(Boolean value)
        {
            _trackerClass.CallStatic("setDebugMode", value);
        }

        void ITracker.TrackEvent(String name, IDictionary<String, String> eventParams)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }

            using (var nameString = JavaHelper.CreateJavaString(name))
            {
                if (eventParams == null || eventParams.Count == 0)
                {
                    _trackerClass.CallStatic("trackEvent", nameString);
                }
                else
                {
                    using (var eventParamsMap = JavaHelper.CreateJavaStringMap(eventParams))
                    {
                        _trackerClass.CallStatic("trackEvent", nameString, eventParamsMap);
                        return;
                    }
                }
            }
        }

        void ITracker.TrackInviteEvent(IDictionary<String, String> eventParams)
        {
            if (eventParams == null || eventParams.Count == 0)
            {
                _trackerClass.CallStatic("trackInviteEvent");
            }
            else
            {
                using (var eventParamsMap = JavaHelper.CreateJavaStringMap(eventParams))
                {
                    _trackerClass.CallStatic("trackInviteEvent", eventParamsMap);
                }
            }
        }

        void ITracker.TrackLevelEvent(Int32? level, IDictionary<String, String> eventParams)
        {
            if (!level.HasValue)
            {
                if (eventParams == null || eventParams.Count == 0)
                {
                    _trackerClass.CallStatic("trackLevelEvent");
                }
                else
                {
                    using (var eventParamsMap = JavaHelper.CreateJavaStringMap(eventParams))
                    {
                        _trackerClass.CallStatic("trackLevelEvent", eventParamsMap);
                    }
                }
            }

            using (var eventParamsMap = JavaHelper.CreateJavaStringMap(eventParams ?? new Dictionary<String, String>()))
            {
                _trackerClass.CallStatic("trackLevelEvent", level.Value, eventParamsMap);
            }
        }

        void ITracker.TrackLoginEvent(String userId, String vkConnectId, IDictionary<String, String> eventParams)
        {
            if (eventParams == null || eventParams.Count == 0)
            {
                _trackerClass.CallStatic("trackLoginEvent", userId, vkConnectId);
            }
            else
            {
                using (var eventParamsMap = JavaHelper.CreateJavaStringMap(eventParams))
                {
                    _trackerClass.CallStatic("trackLoginEvent", userId, vkConnectId, eventParamsMap);
                }
            }
        }

        void ITracker.TrackRegistrationEvent(String userId, String vkConnectId, IDictionary<String, String> eventParams)
        {
            if (eventParams == null || eventParams.Count == 0)
            {
                _trackerClass.CallStatic("trackRegistrationEvent", userId, vkConnectId);
            }
            else
            {
                using (var eventParamsMap = JavaHelper.CreateJavaStringMap(eventParams))
                {
                    _trackerClass.CallStatic("trackRegistrationEvent", userId, vkConnectId, eventParamsMap);
                }
            }
        }

        void ITracker.TrackAdEvent(AdEvent adEvent)
        {
            AndroidJavaObject builder;
            switch (adEvent.appEvent)
            {
                case AppEventEnum.AdClick:
                    builder = _adEventBuilderClass.CallStatic<AndroidJavaObject>("newClickBuilder", (int)adEvent.network);
                    break;
                case AppEventEnum.AdImpression:
                    builder = _adEventBuilderClass.CallStatic<AndroidJavaObject>("newImpressionBuilder", (int)adEvent.network);
                    break;
                case AppEventEnum.AdRevenue:
                    builder = _adEventBuilderClass.CallStatic<AndroidJavaObject>("newRevenueBuilder",
                                                                                 (int)adEvent.network,
                                                                                 adEvent.revenue,
                                                                                 adEvent.currency);
                    break;
                default:
                    LibraryLogger.Log("adEvent unsupported event type");
                    return;
            }

            AndroidJavaObject adEventObject = builder.Call<AndroidJavaObject>("withSource", adEvent.source)
                                                     .Call<AndroidJavaObject>("withPlacementId", adEvent.placementId)
                                                     .Call<AndroidJavaObject>("withAdId", adEvent.adId)
                                                     .Call<AndroidJavaObject>("withAdFormat", adEvent.adFormat)
                                                     .Call<AndroidJavaObject>("build");

            _trackerClass.CallStatic("trackAdEvent", adEventObject);
        }

        void ITracker.Flush()
        {
            _trackerClass.CallStatic("flush");
        }

        MyTrackerParams ITracker.MyTrackerParams
        {
            get { return _myTrackerParams; }
        }

        MyTrackerConfig ITracker.MyTrackerConfig
        {
            get { return _myTrackerConfig; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private sealed class AttributionListenerImpl : AndroidJavaProxy
        {
            private readonly Tracker _tracker;

            public AttributionListenerImpl(Tracker tracker)
                : base("com.my.tracker.MyTracker$AttributionListener")
            {
                _tracker = tracker;
            }

            public void onReceiveAttribution(AndroidJavaObject attributionJavaObject)
            {
                if (attributionJavaObject == null)
                {
                    return;
                }

                var listener = _tracker._attributionListener;
                if (listener == null)
                {
                    return;
                }

                var deeplink = attributionJavaObject.Call<String>("getDeeplink");
                if (deeplink == null)
                {
                    return;
                }

                listener(new MyTrackerAttribution(deeplink));
            }
        }

#if UNITY_ANDROID
        void ITracker.TrackPurchaseEvent(String skuDetails, String purchaseData, String dataSignature, IDictionary<String, String> eventParams)
        {
            using (var skuDetailsJson = JavaHelper.CreateJavaJsonObbject(skuDetails))
            {
                using (var purchaseDataJson = JavaHelper.CreateJavaJsonObbject(purchaseData))
                {
                    using (var dataSignatureString = JavaHelper.CreateJavaString(dataSignature))
                    {
                        if (eventParams == null || eventParams.Count == 0)
                        {
                            _trackerClass.CallStatic("trackPurchaseEvent", skuDetailsJson, purchaseDataJson, dataSignatureString);
                        }
                        else
                        {
                            using (var eventParamsMap = JavaHelper.CreateJavaStringMap(eventParams))
                            {
                                _trackerClass.CallStatic("trackPurchaseEvent", skuDetailsJson, purchaseDataJson, dataSignatureString, eventParamsMap);
                            }
                        }
                    }
                }
            }
        }
#endif

#if UNITY_PURCHASING
        void ITracker.TrackPurchaseEvent(UnityEngine.Purchasing.Product product, IDictionary<String, String> eventParams)
        {
            if (product == null)
            {
                return;
            }

            if (product.receipt == null)
            {
                return;
            }

            var receiptWrapper = (Dictionary<String, System.Object>)UnityEngine.Purchasing.MiniJson.JsonDecode(product.receipt);
            var payloadWrapper = (Dictionary<String, System.Object>)UnityEngine.Purchasing.MiniJson.JsonDecode((String)receiptWrapper["Payload"]);

            var json = (String)payloadWrapper["json"];
            var signature = (String)payloadWrapper["signature"];

            var skuDetailsModel = new Dictionary<String, String>()
            {
                {PriceAmountKey,  (product.metadata.localizedPrice * PriceMultiplier).ToString(CultureInfo.InvariantCulture)},
                {PriceCurrencyCode,  product.metadata.isoCurrencyCode}
            };

            var skuDetails = UnityEngine.Purchasing.MiniJson.JsonEncode(skuDetailsModel);
            using (var skuDetailsJson = JavaHelper.CreateJavaJsonObbject(skuDetails))
            {
                using (var payloadJson = JavaHelper.CreateJavaJsonObbject(json))
                {
                    using (var signatureString = JavaHelper.CreateJavaString(signature))
                    {
                        if (eventParams == null || eventParams.Count == 0)
                        {
                            _trackerClass.CallStatic("trackPurchaseEvent", skuDetailsJson, payloadJson, signatureString);
                        }
                        else
                        {
                            using (var eventParamsMap = JavaHelper.CreateJavaStringMap(eventParams))
                            {
                                _trackerClass.CallStatic("trackPurchaseEvent", skuDetailsJson, payloadJson, signatureString, eventParamsMap);
                            }
                        }
                    }
                }
            }
        }
#endif
    }
}

#endif