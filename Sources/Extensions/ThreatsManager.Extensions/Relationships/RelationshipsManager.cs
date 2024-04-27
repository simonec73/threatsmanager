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

        public bool IsInitialized => _model != null && _schemaManager != null;

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
        public Guid GetMainAlternativeId()
        {
            return _schemaManager.GetAlternatives(_reference)?.Main ?? Guid.Empty;
        }

        [InitializationRequired]
        public IMitigation GetMainAlternative()
        {
            IMitigation result = null;

            var mainId = GetMainAlternativeId();
            if (mainId != Guid.Empty)
            {
                result = _model.GetMitigation(mainId);
            }

            return result;
        }

        [InitializationRequired]
        public bool SetMainAlternative()
        {
            return AddAlternative(_reference, true);
        }

        public bool SetMainAlternative(IMitigation mitigation)
        {
            return AddAlternative(mitigation, true);
        }

        public bool SetMainAlternative(Guid mitigationId)
        {
            return AddAlternative(mitigationId, true);
        }

        [InitializationRequired]
        public bool ResetMainAlternative()
        {
            bool result = false;

            using (var scope = UndoRedoManager.OpenScope("Reset main alternative"))
            {
                var alternatives = _schemaManager.GetAlternatives(_reference);
                if (alternatives != null)
                {
                    alternatives.Main = Guid.Empty;
                    scope?.Complete();
                    result = true;
                }
            }

            return result;
        }

        [InitializationRequired]
        public Guid GetMainComplementaryId()
        {
            return _schemaManager.GetComplementary(_reference)?.Main ?? Guid.Empty;
        }

        [InitializationRequired]
        public IMitigation GetMainComplementary()
        {
            IMitigation result = null;

            var mainId = GetMainComplementaryId();
            if (mainId != Guid.Empty)
            {
                result = _model.GetMitigation(mainId);
            }

            return result;
        }

        [InitializationRequired]
        public bool SetMainComplementary()
        {
            return AddComplementary(_reference, true);
        }

        public bool SetMainComplementary(IMitigation mitigation)
        {
            return AddComplementary(mitigation, true);
        }

        public bool SetMainComplementary(Guid mitigationId)
        {
            return AddComplementary(mitigationId, true);
        }

        [InitializationRequired]
        public bool ResetMainComplementary()
        {
            bool result = false;

            using (var scope = UndoRedoManager.OpenScope("Reset main complementary"))
            {
                var complementary = _schemaManager.GetComplementary(_reference);
                if (complementary != null)
                {
                    complementary.Main = Guid.Empty;
                    scope?.Complete();
                    result = true;
                }
            }

            return result;
        }

        [InitializationRequired]
        public IEnumerable<Guid> GetAlternativeIDs()
        {
            return _schemaManager.GetAlternatives(_reference)?.MitigationIds;
        }

        [InitializationRequired]
        public IEnumerable<IMitigation> GetAlternatives()
        {
            return GetAlternativeIDs()?
                .Select(x => _model.GetMitigation(x))
                .Where(x => x != null);
        }

        [InitializationRequired]
        public IEnumerable<Guid> GetComplementaryIDs()
        {
            return _schemaManager.GetComplementary(_reference)?.MitigationIds;
        }

        [InitializationRequired]
        public IEnumerable<IMitigation> GetComplementary()
        {
            return GetComplementaryIDs()?
                .Select(x => _model.GetMitigation(x))
                .Where(x => x != null);
        }

        public bool AddAlternative(IMitigation mitigation, bool isMain = false) 
        {
            return AddAlternative(mitigation.Id, isMain);
        }

        [InitializationRequired]
        public bool AddAlternative(Guid mitigationId, bool isMain = false)
        {
            bool result = true;

            using (var scope = UndoRedoManager.OpenScope("Add Alternative"))
            {
                var alternatives = _schemaManager.GetAlternatives(_reference);

                if (alternatives == null)
                {
                    alternatives = new RelationshipDetails();

                    if (!alternatives.AddMitigation(mitigationId))
                    {
                        result = false;
                    }

                    if (result)
                        _schemaManager.SetAlternatives(_reference, alternatives);
                }
                else
                {
                    var otherMitigations = Add(alternatives, mitigationId, isMain);

                    if (otherMitigations?.Any() ?? false)
                    {
                        foreach (var om in otherMitigations)
                        {
                            var omAlt = _schemaManager.GetAlternatives(om);
                            if (omAlt != null)
                            {
                                if (!omAlt.MitigationIds.Contains(mitigationId))
                                {
                                    if (!omAlt.AddMitigation(mitigationId))
                                    {
                                        result = false;
                                        break;
                                    }
                                }

                                if (isMain)
                                    omAlt.Main = mitigationId;
                            }
                        }
                    }
                }

                if (result)
                    scope?.Complete();
            }

            return result;
        }

        public bool RemoveAlternative(IMitigation mitigation)
        {
            return RemoveAlternative(mitigation.Id);
        }

        [InitializationRequired]
        public bool RemoveAlternative(Guid mitigationId)
        {
            bool result = false;

            using (var scope = UndoRedoManager.OpenScope("Remove Alternative"))
            {
                var alternatives = _schemaManager.GetAlternatives(_reference);

                result = alternatives?.RemoveMitigation(mitigationId) ?? false;

                if (alternatives.Main == mitigationId)
                    alternatives.Main = Guid.Empty;

                scope?.Complete();
            }

            return result;
        }

        public bool AddComplementary(IMitigation mitigation, bool isMain = false)
        {
            return AddComplementary(mitigation.Id, isMain);
        }

        [InitializationRequired]
        public bool AddComplementary(Guid mitigationId, bool isMain = false)
        {
            bool result = true;

            using (var scope = UndoRedoManager.OpenScope("Add Complementary"))
            {
                var complementary = _schemaManager.GetComplementary(_reference);

                if (complementary == null)
                {
                    complementary = new RelationshipDetails();

                    if (!complementary.AddMitigation(mitigationId))
                    {
                        result = false;
                    }

                    if (result)
                        _schemaManager.SetComplementary(_reference, complementary);
                }
                else
                {
                    var otherMitigations = Add(complementary, mitigationId, isMain);

                    if (otherMitigations?.Any() ?? false)
                    {
                        foreach (var om in otherMitigations)
                        {
                            var omAlt = _schemaManager.GetComplementary(om);
                            if (omAlt != null)
                            {
                                if (!omAlt.MitigationIds.Contains(mitigationId))
                                {
                                    if (!omAlt.AddMitigation(mitigationId))
                                    {
                                        result = false;
                                        break;
                                    }
                                }

                                if (isMain)
                                    omAlt.Main = mitigationId;
                            }
                        }
                    }
                }

                if (result)
                    scope?.Complete();
            }

            return result;
        }

        public bool RemoveComplementary(IMitigation mitigation)
        {
            return RemoveComplementary(mitigation.Id);
        }

        [InitializationRequired]
        public bool RemoveComplementary(Guid mitigationId)
        {
            bool result = false;

            using (var scope = UndoRedoManager.OpenScope("Remove Complementary"))
            {
                var complementary = _schemaManager.GetComplementary(_reference);

                result = complementary?.RemoveMitigation(mitigationId) ?? false;

                if (complementary.Main == mitigationId)
                    complementary.Main = Guid.Empty;

                scope?.Complete();
            }

            return result;
        }

        private IMitigation[] Add([NotNull] RelationshipDetails details, Guid mitigationId, bool isMain)
        {
            if (!details.MitigationIds.Contains(mitigationId))
            {
                if (!details.AddMitigation(mitigationId))
                {
                    return null;
                }
            }

            if (isMain)
                details.Main = mitigationId;

            return details.MitigationIds?
                .Where(x => x != mitigationId)
                .Select(x => _model.GetMitigation(x))
                .Where(x => x != null).ToArray();
        }
    }
}
