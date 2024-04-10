using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Extensions.Relationships
{
    public class RelationshipsManager : IInitializableObject
    {
        private readonly IMitigation _reference;
        private readonly IThreatModel _model;
        private readonly RelationshipSchemaManager _schemaManager;

        public RelationshipsManager([NotNull] IMitigation referenceMitigation)
        {
            _reference = referenceMitigation;
            _model = referenceMitigation.Model;
            _schemaManager = new RelationshipSchemaManager(referenceMitigation.Model);
        }

        public bool IsInitialized => _model != null;

        public bool IsMainAlternative
        {
            get
            {
                return (_schemaManager.GetAlternatives(_reference)?.Main ?? Guid.Empty) == _reference.Id;
            }
        }

        public bool IsMainComplementary
        {
            get
            {
                return (_schemaManager.GetComplementary(_reference)?.Main ?? Guid.Empty) == _reference.Id;
            }
        }

        [InitializationRequired]
        public IMitigation GetMainAlternative()
        {
            var mainId = _schemaManager.GetAlternatives(_reference)?.Main ?? Guid.Empty;
            return _model.GetMitigation(mainId);
        }

        [InitializationRequired]
        public bool SetMainAlternative()
        {
            return SetAlternative(_reference, true);
        }

        [InitializationRequired]
        public bool SetMainAlternative(IMitigation mitigation)
        {
            return SetAlternative(mitigation, true);
        }

        [InitializationRequired]
        public IMitigation GetMainComplementary()
        {
            var mainId = _schemaManager.GetComplementary(_reference)?.Main ?? Guid.Empty;
            return _model.GetMitigation(mainId);
        }

        [InitializationRequired]
        public bool SetMainComplementary()
        {
            return SetComplementary(_reference, true);
        }

        [InitializationRequired]
        public bool SetMainComplementary(IMitigation mitigation)
        {
            return SetComplementary(mitigation, true);
        }

        [InitializationRequired]
        public IEnumerable<IMitigation> GetAlternatives()
        {
            return _schemaManager.GetAlternatives(_reference)?.MitigationIds?
                .Select(x => _model.GetMitigation(x))
                .Where(x => x != null);
        }

        [InitializationRequired]
        public IEnumerable<IMitigation> GetComplementary()
        {
            return _schemaManager.GetAlternatives(_reference)?.MitigationIds?
                .Select(x => _model.GetMitigation(x))
                .Where(x => x != null);
        }

        [InitializationRequired]
        public bool SetAlternative(IMitigation mitigation, bool isMain = false) 
        {
            bool result = true;

            using (var scope = UndoRedoManager.OpenScope("Set Alternative"))
            {
                var alternatives = _schemaManager.GetAlternatives(_reference);

                if (!alternatives.MitigationIds.Contains(mitigation.Id))
                {
                    if (!alternatives.AddMitigation(mitigation))
                    {
                        return false;
                    }
                }

                if (isMain)
                    alternatives.Main = mitigation.Id;

                var otherMitigations = alternatives.MitigationIds?
                    .Where(x => x != mitigation.Id)
                    .Select(x => _model.GetMitigation(x))
                    .Where(x => x != null).ToArray();

                if (otherMitigations?.Any() ?? false)
                {
                    foreach (var om in otherMitigations)
                    {
                        var omAlt = _schemaManager.GetAlternatives(om);
                        if (omAlt != null)
                        {
                            if (!omAlt.MitigationIds.Contains(mitigation.Id))
                            {
                                if (!omAlt.AddMitigation(mitigation))
                                    return false;
                            }

                            if (isMain)
                                omAlt.Main = mitigation.Id;
                        }
                    }
                }

                scope?.Complete();
            }

            return result;
        }

        [InitializationRequired]
        public bool SetComplementary(IMitigation mitigation, bool isMain = false)
        {
            bool result = true;

            using (var scope = UndoRedoManager.OpenScope("Set Main Alternative"))
            {
                var alternatives = _schemaManager.GetAlternatives(_reference);

                if (!alternatives.MitigationIds.Contains(mitigation.Id))
                {
                    if (!alternatives.AddMitigation(mitigation))
                    {
                        return false;
                    }
                }

                if (isMain)
                    alternatives.Main = mitigation.Id;

                var otherMitigations = alternatives.MitigationIds?
                    .Where(x => x != mitigation.Id)
                    .Select(x => _model.GetMitigation(x))
                    .Where(x => x != null).ToArray();

                if (otherMitigations?.Any() ?? false)
                {
                    foreach (var om in otherMitigations)
                    {
                        var omAlt = _schemaManager.GetAlternatives(om);
                        if (omAlt != null)
                        {
                            if (!omAlt.MitigationIds.Contains(mitigation.Id))
                            {
                                if (!omAlt.AddMitigation(mitigation))
                                    return false;
                            }

                            if (isMain)
                                omAlt.Main = mitigation.Id;
                        }
                    }
                }

                scope?.Complete();
            }

            return result;
        }
    }
}
