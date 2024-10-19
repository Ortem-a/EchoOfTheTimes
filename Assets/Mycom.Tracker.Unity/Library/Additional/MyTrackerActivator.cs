using UnityEngine;
//using Mycom.Tracker.Unity;

namespace Mycom.Tracker.Unity
{
    public class MyTrackerActivator : MonoBehaviour
    {
        public void Awake()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                var myTrackerParams = MyTracker.MyTrackerParams;
                myTrackerParams.CustomUserId = "user_id";

                // Можно настроить конфигурацию трекера
                var myTrackerConfig = MyTracker.MyTrackerConfig;
                myTrackerConfig.BufferingPeriod = 60;

                MyTracker.Init("54639465012691779610");
            }
        }
    }
}

