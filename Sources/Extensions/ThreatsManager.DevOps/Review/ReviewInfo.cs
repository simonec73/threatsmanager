using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Review
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ReviewInfo
    {
        [JsonProperty("text")]
        private string _text { get; set; }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                ModifiedOn = DateTime.Now;
                ModifiedBy = GetUserName();
            }
        }

        [JsonProperty("settledOn")]
        private DateTime _settledOn { get; set; }

        public DateTime SettledOn
        {
            get => _settledOn;
            set
            {
                _settledOn = value;
                ModifiedOn = DateTime.Now;
                ModifiedBy = GetUserName();
            }
        }

        [JsonProperty("settledBy")]
        private string _settledBy { get; set; }

        public string SettledBy
        {
            get => _settledBy;
            set
            {
                _settledBy = value;
                ModifiedOn = DateTime.Now;
                ModifiedBy = GetUserName();
            }
        }
            
        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; private set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; private set; }

        [JsonProperty("modifiedOn")]
        public DateTime ModifiedOn { get; protected set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; protected set; }

        protected string GetUserName()
        {
            string result;
            try
            {
                result = UserName.GetDisplayName();
            }
            catch
            {
                result = Environment.UserName;
            }

            return result;
        }
    }
}
