using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.DevOps.Review
{
    public class BacklogReviewPropertyViewer : IPropertyViewer
    {
        private readonly IPropertiesContainer _container;
        private readonly IProperty _property;

        public BacklogReviewPropertyViewer([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            _container = container;
            _property = property;
        }

        public IEnumerable<IPropertyViewerBlock> Blocks
        {
            get
            {
                IEnumerable<IPropertyViewerBlock> result = null;

                if (_property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is ReviewInfo info)
                {
                    result = new IPropertyViewerBlock[] {
                        new BacklogReviewTextPropertyViewerBlock(info),
                        new BacklogReviewSettledByPropertyViewerBlock(info),
                        new BacklogReviewSettledOnPropertyViewerBlock(info)
                    };
                }

                return result;
            }
        }
    }
}