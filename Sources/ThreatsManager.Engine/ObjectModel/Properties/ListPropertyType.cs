using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.PropertyList, ThreatsManager.Engine")]
    public class ListPropertyType : BaseListPropertyType, IListPropertyType
    {
        public ListPropertyType()
        {

        }

        public ListPropertyType([Required] string name, [NotNull] IPropertySchema schema) : base(name, schema)
        {
        }

        #region Specific implementation.
        public override IPropertyType Clone([NotNull] IPropertyTypesContainer container)
        {
            IPropertyType result = null;

            if (container is IPropertySchema schema)
            {
                result = new ListPropertyType
                {
                    _id = _id,
                    _schemaId = schema.Id,
                    Name = Name,
                    Description = Description,
                    _listProviderId = _listProviderId,
                    Context = Context,
                    _cachedList = _cachedList,
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