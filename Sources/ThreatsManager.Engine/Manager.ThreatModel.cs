using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using System.IO;
using ThreatsManager.Engine.ObjectModel;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Interfaces.Exceptions;
using System.Linq;

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
                //_model.SuspendDirty();
                ApplyInitializers();
                _model.RegisterEvents();
            }
            finally
            {
                //_model.ResumeDirty();
            }

            return _model;
        }

        #region Initializers.
        private void ApplyInitializers()
        {
            var initializers = GetExtensions<IInitializer>();
            if (initializers?.Any() ?? false)
            {
                foreach (var initializer in initializers)
                {
                    initializer.Initialize(_model);
                }
            }
        }
        #endregion

        #region Deserialize.
        public static IThreatModel Deserialize(string json, bool ignoreMissingMembers = false)
        {
            IThreatModel result = null;

            if (!string.IsNullOrWhiteSpace(json))
            {
                var binder = new KnownTypesBinder();

                using (var textReader = new StringReader(json))
                using (var reader = new JsonTextReader(textReader))
                {
                    var serializer = new JsonSerializer
                    {
                        TypeNameHandling = TypeNameHandling.None,
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        SerializationBinder = binder,
                        MaxDepth = 128,
                        MissingMemberHandling = ignoreMissingMembers
                            ? MissingMemberHandling.Ignore
                            : MissingMemberHandling.Error
                    };
                    result = serializer.Deserialize<ThreatModel>(reader);
                }
            }

            return result;
        }
        #endregion
    }
}
