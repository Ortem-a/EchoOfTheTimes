#if UNITY_IOS
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Mycom.Tracker.Unity.Internal.Interfaces;
using UnityEngine;

namespace Mycom.Tracker.Unity.Internal.Implementations.iOS
{
    internal sealed class TrackerParams : ITrackerParams
    {
        private static String[] DeserializeFromString(String value)
        {
            if (value == null)
            {
                return null;
            }

            LibraryLogger.Log("Deserialize from string " + value);

            return value.TrimStart('[')
                        .TrimEnd(']')
                        .Trim()
                        .Split(',')
                        .Select(s => s.Trim('"'))
                        .ToArray();
        }

        private static String SerializeToString(String[] value)
        {
            if (value == null)
            {
                return null;
            }

            var result = "[" + String.Join(",", value.Select(s => '"' + s + '"').ToArray()) + "]";

            LibraryLogger.Log("Serialize to string result " + result);

            return result;
        }

        [DllImport("__Internal")]
        private static extern Int32 MRMTMyTrackerParamsGetAge();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerParamsGetCustomUserIds();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerParamsGetEmails();

        [DllImport("__Internal")]
        private static extern Int32 MRMTMyTrackerParamsGetGender();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerParamsGetIcqIds();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerParamsGetLang();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerParamsGetMrgsAppId();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerParamsGetMrgsId();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerParamsGetMrgsUserId();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerParamsGetOkIds();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerParamsGetPhones();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerParamsGetVkIds();

        [DllImport("__Internal")]
        private static extern String MRMTMyTrackerParamsGetVkConnectIds();

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetAge(Int32 value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetCustomUserIds(String value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetEmails(String value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetGender(Int32 value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetIcqIds(String value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetLang(String value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetMrgsAppId(String value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetMrgsId(String value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetMrgsUserId(String value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetOkIds(String value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetPhones(String value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetVkIds(String value);

        [DllImport("__Internal")]
        private static extern void MRMTMyTrackerParamsSetVkConnectIds(String value);

        void IDisposable.Dispose() { }

        Int32 ITrackerParams.GetAge()
        {
            return MRMTMyTrackerParamsGetAge();
        }

        String[] ITrackerParams.GetCustomUserIds()
        {
            return DeserializeFromString(MRMTMyTrackerParamsGetCustomUserIds());
        }

        String[] ITrackerParams.GetEmails()
        {
            return DeserializeFromString(MRMTMyTrackerParamsGetEmails());
        }

        GenderEnum ITrackerParams.GetGender()
        {
            return (GenderEnum) MRMTMyTrackerParamsGetGender();
        }

        String[] ITrackerParams.GetIcqIds()
        {
            return DeserializeFromString(MRMTMyTrackerParamsGetIcqIds());
        }

        String ITrackerParams.GetLang()
        {
            return MRMTMyTrackerParamsGetLang();
        }

        String ITrackerParams.GetMrgsAppId()
        {
            return MRMTMyTrackerParamsGetMrgsAppId();
        }

        String ITrackerParams.GetMrgsId()
        {
            return MRMTMyTrackerParamsGetMrgsId();
        }

        String ITrackerParams.GetMrgsUserId()
        {
            return MRMTMyTrackerParamsGetMrgsUserId();
        }

        String[] ITrackerParams.GetOkIds()
        {
            return DeserializeFromString(MRMTMyTrackerParamsGetOkIds());
        }

        String[] ITrackerParams.GetPhones()
        {
            return DeserializeFromString(MRMTMyTrackerParamsGetPhones());
        }

        String[] ITrackerParams.GetVkIds()
        {
            return DeserializeFromString(MRMTMyTrackerParamsGetVkIds());
        }

        String[] ITrackerParams.GetVkConnectIds()
        {
            return DeserializeFromString(MRMTMyTrackerParamsGetVkConnectIds());
        }

        void ITrackerParams.SetAge(Int32 value)
        {
            MRMTMyTrackerParamsSetAge(value);
        }


        void ITrackerParams.SetCustomUserIds(String[] value)
        {
            MRMTMyTrackerParamsSetCustomUserIds(SerializeToString(value));
        }

        void ITrackerParams.SetEmails(String[] value)
        {
            MRMTMyTrackerParamsSetEmails(SerializeToString(value));
        }

        void ITrackerParams.SetGender(GenderEnum value)
        {
            MRMTMyTrackerParamsSetGender((Int32) value);
        }

        void ITrackerParams.SetIcqIds(String[] value)
        {
            MRMTMyTrackerParamsSetIcqIds(SerializeToString(value));
        }

        void ITrackerParams.SetLang(String value)
        {
            MRMTMyTrackerParamsSetLang(value);
        }

        void ITrackerParams.SetMrgsAppId(String value)
        {
            MRMTMyTrackerParamsSetMrgsAppId(value);
        }

        void ITrackerParams.SetMrgsId(String value)
        {
            MRMTMyTrackerParamsSetMrgsId(value);
        }

        void ITrackerParams.SetMrgsUserId(String value)
        {
            MRMTMyTrackerParamsSetMrgsUserId(value);
        }

        void ITrackerParams.SetOkIds(String[] value)
        {
            MRMTMyTrackerParamsSetOkIds(SerializeToString(value));
        }

        void ITrackerParams.SetPhones(String[] value)
        {
            MRMTMyTrackerParamsSetPhones(SerializeToString(value));
        }

        void ITrackerParams.SetVkIds(String[] value)
        {
            MRMTMyTrackerParamsSetVkIds(SerializeToString(value));
        }

        void ITrackerParams.SetVkConnectIds(String[] value)
        {
            MRMTMyTrackerParamsSetVkConnectIds(SerializeToString(value));
        }
   }
}

#endif