using System;
using System.Collections.Generic;
using System.Threading;
using Mycom.Tracker.Unity.Ads;
using Mycom.Tracker.Unity.Internal;
using Mycom.Tracker.Unity.Internal.Interfaces;

#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

namespace Mycom.Tracker.Unity
{
    /// <summary>
    /// Main facade to access MyTracker API
    /// </summary>
    public static class MyTracker
    {
        private static readonly ITracker Tracker = PlatformFactory.CreateTracker();
        // 0 - initial state
        // 1 - initialized
        private static Int32 State;

        /// <summary>
        /// Gets the instance id
        /// </summary>
        public static String InstanceId
        {
            get { return Tracker.GetInstanceId(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tracker is in debug mode
        /// </summary>
        public static Boolean IsDebugMode
        {
            get { return Tracker.IsDebugMode(); }
            set { Tracker.SetDebugMode(value); }
        }

        /// <summary>
        /// Gets <see cref="MyTrackerParams"/> object
        /// </summary>
        public static MyTrackerParams MyTrackerParams
        {
            get { return Tracker.MyTrackerParams; }
        }

        /// <summary>
        /// Gets <see cref="MyTrackerConfig"/> object 
        /// </summary>
        public static MyTrackerConfig MyTrackerConfig
        {
            get { return Tracker.MyTrackerConfig; }
        }

        /// <summary>
        /// Perform initialization of tracker
        /// </summary>
        /// <remarks>This method should be called right after setup tracker configuration</remarks> 
        /// <param name="id">The identifier of your application</param>
        public static void Init(String id)
        {
            if (Interlocked.CompareExchange(ref State, 1, 0) != 0)
            {
                LibraryLogger.Log("MyTracker has been already created");
                return;
            }

            if (String.IsNullOrEmpty(id))
            {
                LibraryLogger.Log("id parameter is null");
                return;
            }

            LibraryLogger.Log("MyTracker unity package version " + SDKVersion.Version);

            Tracker.Init(id);
        }

        /// <summary>
        /// Set the attribution listener
        /// </summary>
        /// <param name="listener">Callback for attribution</param>
        public static void SetAttributionListener(Action<MyTrackerAttribution> listener)
        {
            Tracker.SetAttributionListener(listener);
        }

        /// <summary>
        /// Track user defined event with custom name and optional key-value parameters
        /// </summary>
        /// <param name="name"> User defined event name. Max length is 64 symbols</param>
        /// <param name="eventParams">Additional event key-value parameters. Max length for key or value is 64 symbols</param>
        public static void TrackEvent(String name, IDictionary<String, String> eventParams = null)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }

            Tracker.TrackEvent(name, eventParams);
        }

        /// <summary>
        /// Create an invitation event
        /// </summary>
        /// <param name="eventParams">Additional event key-value parameters. Max length for key or value is 64 symbols</param>
        public static void TrackInviteEvent(IDictionary<String, String> eventParams = null)
        {
            Tracker.TrackInviteEvent(eventParams);
        }

        /// <summary>
        /// Track achieving new level
        /// </summary>
        /// <remarks>Call this method when user has achieved new level</remarks>
        /// <param name="level">A level value</param>
        /// <param name="eventParams">Additional event key-value parameters. Max length for key or value is 64 symbols</param>
        public static void TrackLevelEvent(Int32? level = null, IDictionary<String, String> eventParams = null)
        {
            Tracker.TrackLevelEvent(level, eventParams);
        }

        /// <summary>
        /// Track user login event
        /// </summary>
        /// <remarks>Call the method right after user successfully authorized in the app and got an unique identifier</remarks>
        /// <param name="userId">Unique user identifier</param>
        /// <param name="vkConnectId">VK Connect identifier</param>
        /// <param name="eventParams">Additional event key-value parameters. Max length for key or value is 64 symbols</param>
        public static void TrackLoginEvent(String userId, String vkConnectId, IDictionary<String, String> eventParams = null)
        {
            Tracker.TrackLoginEvent(userId, vkConnectId, eventParams);
        }

        /// <summary>
        /// Track user registration event
        /// </summary>
        /// <remarks>Call the method right after user successfully authorized in the app and got an unique identifier</remarks>
        /// <param name="userId">Unique user identifier</param>
        /// <param name="vkConnectId">VK Connect identifier</param>
        /// <param name="eventParams">Additional event key-value parameters. Max length for key or value is 64 symbols</param>
        public static void TrackRegistrationEvent(String userId, String vkConnectId, IDictionary<String, String> eventParams = null)
        {
            Tracker.TrackRegistrationEvent(userId, vkConnectId, eventParams);
        }

        /// <summary>
        /// Track advertising event
        /// </summary>
        /// <remarks>Call this method when an advertising event has occurred</remarks>
        /// <param name="adEvent">Instance of <see cref="AdEvent"/></param>
        public static void TrackAdEvent(AdEvent adEvent)
        {
            Tracker.TrackAdEvent(adEvent);
        }

        /// <summary>
        /// Force sending events
        /// </summary>
        public static void Flush()
        {
            Tracker.Flush();
        }

#if UNITY_ANDROID
        /// <summary>
        /// Track purchase event
        /// </summary>
        /// <param name="skuDetails">SkuDetails json string</param>
        /// <param name="purchaseData">PurchaseData json string</param>
        /// <param name="dataSignature">DataSignature</param>
        /// <param name="eventParams">Additional event key-value parameters. Max length for key or value is 64 symbols</param>
        public static void TrackPurchaseEvent(String skuDetails, String purchaseData, String dataSignature, IDictionary<String, String> eventParams = null)
        {
            Tracker.TrackPurchaseEvent(skuDetails, purchaseData, dataSignature, eventParams);
        }
#endif

#if UNITY_PURCHASING
        /// <summary>
        /// Track purchase event
        /// </summary>
        /// <param name="product">Purchased product</param>
        /// <param name="eventParams">Additional event key-value parameters. Max length for key or value is 64 symbols</param>
        public static void TrackPurchaseEvent(Product product, IDictionary<String, String> eventParams = null)
        {
            Tracker.TrackPurchaseEvent(product, eventParams);
        }
#endif
    }
}