using System;

namespace Mycom.Tracker.Unity
{
    /// <summary>
    /// Class - wrapper of attribution deeplink
    /// </summary>
    /// <remarks>
    /// The instance of this class will be received in <see cref="MyTracker.SetAttributionListener(Action{MyTrackerAttribution})"/>
    /// </remarks>
    public sealed class MyTrackerAttribution
    {
        /// <summary>
        /// Gets an attribution deeplink value
        /// </summary>
        public String Deeplink { get; private set; }

        internal MyTrackerAttribution(String deeplink)
        {
            Deeplink = deeplink;
        }
    }
}