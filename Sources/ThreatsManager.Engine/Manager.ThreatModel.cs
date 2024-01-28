using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using System.IO;
using System.Reflection;
using System.Text;
using System;
using ThreatsManager.Engine.ObjectModel;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities.Exceptions;
using ThreatsManager.Utilities;
using ThreatsManager.Interfaces.Exceptions;

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

        #region Deserialize.
        public static IThreatModel Deserialize(string json, bool ignoreMissingMembers = false)
        {
            IThreatModel result = null;

            if (!string.IsNullOrWhiteSpace(json))
            {
                try
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
                catch (JsonSerializationException e)
                {
                    throw new ThreatModelOpeningFailureException("A serialization issue has occurred.", e);
                }
            }

            return result;
        }
        #endregion
    }
}
