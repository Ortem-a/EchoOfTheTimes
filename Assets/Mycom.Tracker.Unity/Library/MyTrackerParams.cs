using System;
using System.Linq;
using Mycom.Tracker.Unity.Internal.Interfaces;

namespace Mycom.Tracker.Unity
{
    /// <summary>
    /// Class for specifying additional tracking parameters
    /// </summary>
    public sealed class MyTrackerParams
    {
        private readonly ITrackerParams _trackerParams;

        /// <summary>
        /// Gets or sets the user's age
        /// </summary>
        public Int32 Age
        {
            get { return _trackerParams.GetAge(); }
            set { _trackerParams.SetAge(value); }
        }

        /// <summary>
        /// Gets or sets an additional user's ID
        /// </summary>
        public String CustomUserId
        {
            get
            {
                var values = _trackerParams.GetCustomUserIds();
                return values != null ? values.FirstOrDefault() : null;
            }
            set { _trackerParams.SetCustomUserIds(value == null ? null : new[] { value }); }
        }

        /// <summary>
        /// Gets or sets additional user's IDs
        /// </summary>
        public String[] CustomUserIds
        {
            get { return _trackerParams.GetCustomUserIds(); }
            set { _trackerParams.SetCustomUserIds(value); }
        }

        /// <summary>
        /// Gets or sets the user's email
        /// </summary>
        public String Email
        {
            get
            {
                var values = _trackerParams.GetEmails();
                return values != null ? values.FirstOrDefault() : null;
            }
            set { _trackerParams.SetEmails(value == null ? null : new[] { value }); }
        }

        /// <summary>
        /// Gets or sets user's emails
        /// </summary>
        public String[] Emails
        {
            get { return _trackerParams.GetEmails(); }
            set { _trackerParams.SetEmails(value); }
        }

        /// <summary>
        /// Gets or sets the user's gender
        /// </summary>
        public GenderEnum Gender
        {
            get { return _trackerParams.GetGender(); }
            set { _trackerParams.SetGender(value); }
        }

        /// <summary>
        /// Gets or sets the user's ICQ ID
        /// </summary>
        public String IcqId
        {
            get
            {
                var values = _trackerParams.GetIcqIds();
                return values != null ? values.FirstOrDefault() : null;
            }
            set { _trackerParams.SetIcqIds(value == null ? null : new[] { value }); }
        }

        /// <summary>
        /// Gets or sets the user's ICQ IDs
        /// </summary>
        public String[] IcqIds
        {
            get { return _trackerParams.GetIcqIds(); }
            set { _trackerParams.SetIcqIds(value); }
        }

        /// <summary>
        /// Gets or sets a language identifier
        /// </summary>
        public String Lang
        {
            get { return _trackerParams.GetLang(); }
            set { _trackerParams.SetLang(value); }
        }

        public String MrgsAppId
        {
            get { return _trackerParams.GetMrgsAppId(); }
            set { _trackerParams.SetMrgsAppId(value); }
        }

        public String MrgsId
        {
            get { return _trackerParams.GetMrgsId(); }
            set { _trackerParams.SetMrgsId(value); }
        }

        public String MrgsUserId
        {
            get { return _trackerParams.GetMrgsUserId(); }
            set { _trackerParams.SetMrgsUserId(value); }
        }

        /// <summary>
        /// Gets or sets a user's OK ID
        /// </summary>
        public String OkId
        {
            get
            {
                var values = _trackerParams.GetOkIds();
                return values != null ? values.FirstOrDefault() : null;
            }
            set { _trackerParams.SetOkIds(value == null ? null : new[] { value }); }
        }

        /// <summary>
        /// Gets or sets user's OK IDs
        /// </summary>
        public String[] OkIds
        {
            get { return _trackerParams.GetOkIds(); }
            set { _trackerParams.SetOkIds(value); }
        }

        /// <summary>
        /// Gets or sets a user's phone
        /// </summary>
        public String Phone
        {
            get
            {
                var values = _trackerParams.GetPhones();
                return values != null ? values.FirstOrDefault() : null;
            }
            set { _trackerParams.SetPhones(value == null ? null : new[] { value }); }
        }

        /// <summary>
        /// Gets or sets user's phones
        /// </summary>
        public String[] Phones
        {
            get { return _trackerParams.GetPhones(); }
            set { _trackerParams.SetPhones(value); }
        }

        /// <summary>
        /// Gets or sets a user's VK ID
        /// </summary>
        public String VkId
        {
            get
            {
                var values = _trackerParams.GetVkIds();
                return values != null ? values.FirstOrDefault() : null;
            }
            set { _trackerParams.SetVkIds(value == null ? null : new[] { value }); }
        }

        /// <summary>
        /// Gets or sets user's OK IDs
        /// </summary>
        public String[] VkIds
        {
            get { return _trackerParams.GetVkIds(); }
            set { _trackerParams.SetVkIds(value); }
        }

        /// <summary>
        /// Gets or sets user's VK Connect ID
        /// </summary>
        public String VkConnectId
        {
            get
            {
                var values = _trackerParams.GetVkConnectIds();
                return values != null ? values.FirstOrDefault() : null;
            }
            set { _trackerParams.SetVkConnectIds(value == null ? null : new[] { value }); }
        }

        /// <summary>
        /// Gets or sets user's VK Connect IDs
        /// </summary>
        public String[] VkConnectIds
        {
            get { return _trackerParams.GetVkConnectIds(); }
            set { _trackerParams.SetVkConnectIds(value);  }
        }

        internal MyTrackerParams(ITrackerParams trackerParams)
        {
            _trackerParams = trackerParams;
        }
    }
}