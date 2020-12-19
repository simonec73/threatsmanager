using System;
using Newtonsoft.Json;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Annotations
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Annotation
    {
        public Annotation()
        {
            Printable = true;
            CreatedOn = DateTime.Now;
            CreatedBy = GetUserName();
        }

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

        [JsonProperty("printable")]
        public bool Printable { get; protected set; }
            
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