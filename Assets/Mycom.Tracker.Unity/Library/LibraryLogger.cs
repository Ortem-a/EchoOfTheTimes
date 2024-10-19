using System;
using UnityEngine;

namespace Mycom.Tracker.Unity
{
    /// <summary>
    /// Class for logging
    /// </summary>
    public static class LibraryLogger
    {
#if UNITY_IOS && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void MRMTDebugLog(String message);
#endif

        private static readonly String Tag = "[mytracker.unity]: ";

        /// <summary>
        /// Write message to log
        /// </summary>
        public static void Log(String message)
        {
            if(String.IsNullOrEmpty(message))
            {
                return;
            }

#if UNITY_IOS && !UNITY_EDITOR
            MRMTDebugLog(Tag + message);
#else
            Debug.Log(Tag + message);
#endif
        }
    }
}