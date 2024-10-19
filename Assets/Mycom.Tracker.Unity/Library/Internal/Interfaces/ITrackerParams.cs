using System;

namespace Mycom.Tracker.Unity.Internal.Interfaces
{
    internal interface ITrackerParams : IDisposable
    {
        Int32 GetAge();
        void SetAge(Int32 value);

        String[] GetCustomUserIds();
        void SetCustomUserIds(String[] value);

        String[] GetEmails();
        void SetEmails(String[] value);

        GenderEnum GetGender();
        void SetGender(GenderEnum value);

        String[] GetIcqIds();
        void SetIcqIds(String[] value);

        String GetLang();
        void SetLang(String value);

        String GetMrgsAppId();
        void SetMrgsAppId(String value);

        String GetMrgsId();
        void SetMrgsId(String value);

        String GetMrgsUserId();
        void SetMrgsUserId(String value);

        String[] GetOkIds();
        void SetOkIds(String[] value);

        String[] GetPhones();
        void SetPhones(String[] value);

        String[] GetVkIds();
        void SetVkIds(String[] value);

        String[] GetVkConnectIds();
        void SetVkConnectIds(String[] value);
    }
}