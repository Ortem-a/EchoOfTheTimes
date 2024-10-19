using UnityEngine;
//using Mycom.Tracker.Unity;

namespace Mycom.Tracker.Unity
{
    public class MyTrackerActivator : MonoBehaviour
    {
        public void Awake()
        {
#if !UNITY_IOS && !UNITY_ANDROID
        return;
#endif
            var myTrackerParams = MyTracker.MyTrackerParams;
            myTrackerParams.CustomUserId = "user_id";

            // Можно настроить конфигурацию трекера
            var myTrackerConfig = MyTracker.MyTrackerConfig;
            myTrackerConfig.BufferingPeriod = 60;

#if UNITY_IOS
            MyTracker.Init("SDK_KEY_IOS");
#elif UNITY_ANDROID
            MyTracker.Init("54639465012691779610");
#endif
        }
    }
}

