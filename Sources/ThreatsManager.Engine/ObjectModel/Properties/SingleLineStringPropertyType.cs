using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [Serializable]
    [PropertyTypeAspect]
    [IdentityAspect]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.PropertySingleLineString, ThreatsManager.Engine")]
    public class SingleLineStringPropertyType : ISingleLineStringPropertyType
    {
        public SingleLineStringPropertyType()
        {

        }

        public SingleLineStringPropertyType([Required] string name, [NotNull] IPropertySchema schema) : this()
        {
            _id = Guid.NewGuid();
            _schemaId = schema.Id;
            Name = name;
            Visible = true;
        }

        #region Placeholder for ISingleLineStringPropertyType.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid SchemaId { get; }
        public int Priority { get; set; }
        public bool Visible { get; set; }
        #endregion
        
        #region Additional placeholders required.
        protected Guid _id { get; set; }
        protected Guid _schemaId { get; set; }
        #endregion

        #region Specific implementation.
        public override string ToString()
        {
            return Name;
        }

        public IPropertyType Clone([NotNull] IPropertyTypesContainer container)
        {
            IPropertyType result = null;

            if (container is IPropertySchema schema)
            {
                result = new SingleLineStringPropertyType
                {
                    _id = _id,
                    _schemaId = schema.Id,
                    Name = Name,
                    Description = Description,
                    Visible = Visible,
                    Priority = Priority
                };
                container.Add(result);
            }
            return result;
        }
        #endregion
    }
}
