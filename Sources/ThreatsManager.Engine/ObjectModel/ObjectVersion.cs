using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using System;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.ObjectModel
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class ObjectVersion : IObjectVersion
    {
        private const string DateFormat = "yyyyMMdd.HHmm";
        public ObjectVersion() 
        {
            VersionId = DateTime.Now.ToUniversalTime().ToString(DateFormat);
            VersionAuthor = UserName.GetDisplayName();
        }

        public ObjectVersion([Required] string author, DateTime modifiedDate)
        {
            VersionId = modifiedDate.ToUniversalTime().ToString(DateFormat);
            VersionAuthor = author;
        }

        [JsonProperty("id")]
        public string VersionId { get; private set; }

        [JsonProperty("author")]
        public string VersionAuthor { get; private set; }
    }
}
