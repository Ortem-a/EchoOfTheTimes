using System;

namespace Mycom.Tracker.Unity.Ads
{
    /// <summary>Class describes an advertising event</summary>
    public sealed class AdEvent : MyTrackerEvent
    {
        /// <summary>Advertising network identifier <see cref="AdNetworkEnum"/></summary>
        public readonly AdNetworkEnum network;

        /// <summary>Revenue value</summary>
        public readonly double revenue;

        /// <summary>Currency code</summary>
        public readonly String currency;

        /// <summary>Original source</summary>
        public readonly String source;

        /// <summary>Placement identifier</summary>
        public readonly String placementId;

        /// <summary>Advertising identifier</summary>
        public readonly String adId;

        /// <summary>Advertising format</summary>
        /// <remarks>Some possible values are defined at <see cref="Ads.AdFormat"/></remarks>
        public readonly String adFormat;

        internal AdEvent(AppEventEnum appEvent,
                         AdNetworkEnum network,
                         double revenue,
                         String currency,
                         String source,
                         String placementId,
                         String adId,
                         String adFormat)
                : base(appEvent)
        {
            this.network = network;
            this.revenue = revenue;
            this.currency = currency;
            this.source = source;
            this.placementId = placementId;
            this.adId = adId;
            this.adFormat = adFormat;
        }
    }
}