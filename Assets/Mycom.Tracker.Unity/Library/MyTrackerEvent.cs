namespace Mycom.Tracker.Unity
{
    /// <summary>
    /// Base class for all events
    /// </summary>
    public abstract class MyTrackerEvent
    {
        internal readonly AppEventEnum appEvent;

        internal MyTrackerEvent(AppEventEnum appEvent)
        {
            this.appEvent = appEvent;
        }
    }
}