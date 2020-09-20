using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.ObjectModel;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Engine
{
    public partial class Manager
    {
        private ThreatModel _model;

        public IThreatModel CreateNewThreatModel([Required] string name)
        {
            _model = new ThreatModel(name);

            try
            {
                _model.SuspendDirty();
                ApplyInitializers();
                _model.RegisterEvents();
            }
            finally
            {
                _model.ResumeDirty();
            }

            return _model;
        }

        #region Initializers.
        private void ApplyInitializers()
        {
            var extensions = Instance.Configuration.EnabledExtensions;

            foreach (var id in extensions)
            {
                var metadata = GetExtensionMetadata(id);
                if (metadata != null)
                {
                    GetExtension<IInitializer>(id)?.Initialize(_model);
                }
            }
        }
        #endregion
    }
}
