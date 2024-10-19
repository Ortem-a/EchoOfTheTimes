using System;
using Mycom.Tracker.Unity.Internal.Interfaces;

namespace Mycom.Tracker.Unity
{
    /// <summary>
    /// Class for configuring myTracker
    /// </summary>
    public class MyTrackerConfig
    {
        private readonly ITrackerConfig _trackerConfig;

        /// <summary>
        /// Gets the unique ID of the tracker
        /// </summary>
        public String Id
        {
            get { return _trackerConfig.GetId(); }
        }

        /// <summary>
        /// Gets or sets buffering period 
        /// </summary>
        public Int32 BufferingPeriod
        {
            get { return _trackerConfig.GetBufferingPeriod(); }
            set { _trackerConfig.SetBufferingPeriod(value); }
        }

        /// <summary>
        /// Gets or sets the forcing period.
        /// </summary>
        /// <value>The forcing period.</value>
        public Int32 ForcingPeriod
        {
            get { return _trackerConfig.GetForcingPeriod(); }
            set { _trackerConfig.SetForcingPeriod(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether tracking evironment is enabled
        /// </summary>
        public Boolean IsTrackingEnvironmentEnabled
        {
            get { return _trackerConfig.IsTrackingEnvironmentEnabled(); }
            set { _trackerConfig.SetTrackingEnvironmentEnabled(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether tracking launch is enabled
        /// </summary>
        public Boolean IsTrackingLaunchEnabled
        {
            get { return _trackerConfig.IsTrackingLaunchEnabled(); }
            set { _trackerConfig.SetTrackingLaunchEnabled(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether tracking location is enabled
        /// </summary>
        public Boolean IsTrackingLocationEnabled
        {
            get { return _trackerConfig.IsTrackingLocationEnabled(); }
            set { _trackerConfig.SetTrackingLocationEnabled(value); }
        }
        /// <summary>
        /// Gets or sets a timeout in seconds between application launches
        /// The value is used to determine new session
        /// </summary>
        public Int32 LaunchTimeout
        {
            get { return _trackerConfig.GetLaunchTimeout(); }
            set { _trackerConfig.SetLaunchTimeout(value); }
        }

        /// <summary>
        /// Gets or sets current proxy host
        /// </summary>
        public String ProxyHost
        {
            set { _trackerConfig.SetProxyHost(value); }
        }

        /// <summary>
        /// Managing registering app's install as a result of an ad. Default value is true.
        /// </summary>
        public Boolean RegisterForSkAdAttribution
        {
            set { _trackerConfig.SetRegisterForSkAdAttribution(value); }
            get { return _trackerConfig.IsRegisterForSkAdAttribution(); }
        }

        internal MyTrackerConfig(ITrackerConfig trackerConfig)
        {
            _trackerConfig = trackerConfig;
        }
    }
}