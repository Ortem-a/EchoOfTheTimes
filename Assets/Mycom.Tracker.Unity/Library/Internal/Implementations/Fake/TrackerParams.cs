using Mycom.Tracker.Unity.Internal.Interfaces;

namespace Mycom.Tracker.Unity.Internal.Implementations.Fake
{
    internal sealed class TrackerParams : ITrackerParams
    {
        public void Dispose() { }

        public int GetAge() { return 0; }

        public string[] GetCustomUserIds() { return null; }

        public string[] GetEmails() { return null; }

        public GenderEnum GetGender() { return GenderEnum.None; }

        public string[] GetIcqIds() { return null; }

        public string GetLang() { return null; }

        public string GetMrgsAppId() { return null; }

        public string GetMrgsId() { return null; }

        public string GetMrgsUserId() { return null; }

        public string[] GetOkIds() { return null; }

        public string[] GetPhones() { return null; }

        public string[] GetVkIds() { return null; }

        public string[] GetVkConnectIds() { return null; }

        public void SetAge(int value) { }

        public void SetCustomUserIds(string[] value) { }

        public void SetEmails(string[] value) { }

        public void SetGender(GenderEnum value) { }

        public void SetIcqIds(string[] value) { }

        public void SetLang(string value) { }

        public void SetMrgsAppId(string value) { }

        public void SetMrgsId(string value) { }

        public void SetMrgsUserId(string value) { }

        public void SetOkIds(string[] value) { }

        public void SetPhones(string[] value) { }

        public void SetVkIds(string[] value) { }

        public void SetVkConnectIds(string[] value) { }
    }
}