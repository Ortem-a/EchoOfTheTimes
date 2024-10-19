using System;

namespace Mycom.Tracker.Unity.Ads
{
    /// <summary>Builder-class for <see cref="AdEvent"/></summary>
    public sealed class AdEventBuilder
    {
        /// <summary>Create new ad click event builder</summary>
        /// <param name="network">Advertising network <see cref="AdNetworkEnum"/></param>
        /// <returns>Builder object for creating ad click event</returns>
        public static AdEventBuilder NewClickBuilder(AdNetworkEnum network)
        {
            return new AdEventBuilder(AppEventEnum.AdClick, network, Double.NaN, null);
        }

        /// <summary></summary>
        /// <param name="network">Advertising network <see cref="AdNetworkEnum"/></param>
        /// <returns>Builder object for creating ad impression event</returns>
        public static AdEventBuilder NewImpressionBuilder(AdNetworkEnum network)
        {
            return new AdEventBuilder(AppEventEnum.AdImpression, network, Double.NaN, null);
        }

        /// <summary></summary>
        /// <param name="network">Advertising network <see cref="AdNetworkEnum"/></param>
        /// <param name="revenue">Revenue value</param>
        /// <param name="currency">Currency code in ISO 4217 format</param>
        /// <returns>Builder object for creating ad revenue event</returns>
        public static AdEventBuilder NewRevenueBuilder(AdNetworkEnum network, double revenue, String currency)
        {
            return new AdEventBuilder(AppEventEnum.AdRevenue, network, revenue, currency);
        }

        readonly AppEventEnum appEvent;
        readonly AdNetworkEnum network;
        readonly double revenue;
        readonly String currency;

        String source;
        String placementId;
        String adId;
        String adFormat;

        private AdEventBuilder(AppEventEnum appEvent,
                               AdNetworkEnum network,
                               double revenue,
                               String currency)
        {
            this.appEvent = appEvent;
            this.network = network;
            this.revenue = revenue;
            this.currency = currency;
        }

        /// <summary>Set initial source</summary>
        /// <param name="source">Source value</param>
        /// <returns>Current <see cref="AdEventBuilder"/></returns>
        public AdEventBuilder WithSource(String source)
        {
            this.source = source;
            return this;
        }

        /// <summary>Set placement identifier</summary>
        /// <param name="placementId">Placement identifier value</param>
        /// <returns>Current <see cref="AdEventBuilder"/></returns>
        public AdEventBuilder WithPlacementId(String placementId)
        {
            this.placementId = placementId;
            return this;
        }

        /// <summary>Set advertising identifier</summary>
        /// <param name="adId">Advertising identifier value</param>
        /// <returns>Current <see cref="AdEventBuilder"/></returns>
        public AdEventBuilder WithAdId(String adId)
        {
            this.adId = adId;
            return this;
        }

        /// <summary>Set advertising format</summary>
        /// <param name="adFormat">Advertising format</param>
        /// <remarks>The value could be custom defined or chosen from <see cref="AdFormat"/></remarks>
        /// <returns>Current <see cref="AdEventBuilder"/></returns>
        public AdEventBuilder WithAdFormat(String adFormat)
        {
            this.adFormat = adFormat;
            return this;
        }

        /// <summary>Create new advertising event instance with previously specified values</summary>
        /// <returns>New instance of <see cref="AdEvent"/></returns>
        public AdEvent Build()
        {
            return new AdEvent(appEvent, network, revenue, currency, source, placementId, adId, adFormat);
        }
    }
}