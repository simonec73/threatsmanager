using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.DevOps.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DevOpsFieldMapping
    {
        public DevOpsFieldMapping()
        {
        }

        public DevOpsFieldMapping([Required] string field, [NotNull] IdentityField identityField)
        {
            Field = field;
            _identityFieldType = identityField.FieldType;

            _schemaId = identityField.PropertyType?.SchemaId ?? Guid.Empty;
            _propertyId = identityField.PropertyType?.Id ?? Guid.Empty;
        }

        [JsonProperty("identityFieldType")]
        [JsonConverter(typeof(StringEnumConverter))]
        private IdentityFieldType _identityFieldType { get; set; }

        [JsonProperty("schemaId")]
        private Guid _schemaId { get; set; }

        [JsonProperty("propertyId")]
        private Guid _propertyId { get; set; }

        [JsonProperty("field")]
        public string Field { get; private set; }

        public IdentityField GetMappedField([NotNull] IThreatModel model)
        {
            IdentityField result = null;

            if (_identityFieldType == IdentityFieldType.Property)
            {
                var schema = model.GetSchema(_schemaId);
                var propertyType = schema?.GetPropertyType(_propertyId);
                if (propertyType != null)
                {
                    result = new IdentityField(propertyType);
                }
            }
            else
            {
                result = new IdentityField(_identityFieldType);
            }

            return result;
        }
    }
}