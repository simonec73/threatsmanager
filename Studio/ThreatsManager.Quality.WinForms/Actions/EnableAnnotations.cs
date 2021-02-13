using System.Drawing;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.Schemas;

namespace ThreatsManager.Quality.Actions
{
    [Extension("CB6702CF-1717-419F-A2D0-EE6CA395B036", "Enable Annotations Context Aware Action", 100, ExecutionMode.Management)]
    public class EnableAnnotations : IShapeContextAwareAction, ILinkContextAwareAction, IIdentityContextAwareAction, 
        IThreatEventMitigationContextAwareAction, IThreatTypeMitigationContextAwareAction
    {
        public Scope Scope => Scope.All;
        public string Label => "Enable Annotations";
        public string Group => "Other";
        public Bitmap Icon => Properties.Resources.note_text;
        public Bitmap SmallIcon => Properties.Resources.note_text_small;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IPropertiesContainer container)
            {
                result = Execute(container);
            }

            return result;
        }

        public bool IsVisible(object item)
        {
            bool result = false;

            IThreatModel model = null;
            if (item is IThreatModelChild child)
                model = child.Model;
            else if (item is IThreatModel threatModel)
                model = threatModel;

            if (model != null && item is IPropertiesContainer container)
            {
                var schemaManager = new AnnotationsPropertySchemaManager(model);
                result = !schemaManager.AreAnnotationsEnabled(container);
            }

            return result;
        }

        public bool Execute(IThreatTypeMitigation mitigation)
        {
            bool result = false;

            if (mitigation is IPropertiesContainer container)
            {
                result = Execute(container);
            }

            return result;
        }

        public bool Execute(IThreatEventMitigation identity)
        {
            bool result = false;

            if (identity is IPropertiesContainer container)
            {
                result = Execute(container);
            }

            return result;
        }

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            if (identity is IPropertiesContainer container)
            {
                result = Execute(container);
            }

            return result;
        }

        public bool Execute(IShape shape)
        {
            bool result = false;

            if (shape?.Identity is IPropertiesContainer container)
            {
                result = Execute(container);
            }

            return result;
        }
        public bool Execute(ILink link)
        {
            bool result = false;

            if (link?.DataFlow is IPropertiesContainer container)
            {
                result = Execute(container);
            }

            return result;
        }

        private bool Execute([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            if (container is IThreatModelChild child)
            {
                var schemaManager = new AnnotationsPropertySchemaManager(child.Model);
                result = schemaManager.EnableAnnotations(container);
            }

            return result;
        }
    }
}
