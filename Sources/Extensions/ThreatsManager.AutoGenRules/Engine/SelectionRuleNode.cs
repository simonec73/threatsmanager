﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using ThreatsManager.AutoGenRules.Properties;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public abstract class SelectionRuleNode
    {
        [JsonProperty("modelId")]
        private Guid _modelId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("scope")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Scope Scope { get; set; }

        public virtual Guid ModelId
        {
            get => _modelId;

            set
            {
                if (_modelId != value)
                    _modelId = value;
            }
        }

        protected IThreatModel Model => ModelId != Guid.Empty ? ThreatModelManager.Get(ModelId) : null;

        public override string ToString()
        {
            return Resources.LabelSelectionRuleNode;
        }

        public abstract bool Evaluate(object context);

        protected IIdentity GetScopedIdentity([NotNull] IIdentity identity)
        {
            IIdentity result = null;

            switch (Scope)
            {
                case Scope.Source:
                    if (identity is IDataFlow dataFlowS)
                        result = dataFlowS.Source;
                    break;
                case Scope.Target:
                    if (identity is IDataFlow dataFlowT)
                        result = dataFlowT.Target;
                    break;
                default:
                    result = identity;
                    break;
            }

            return result;
        }

        protected IEnumerable<ITrustBoundary> GetCrossedTrustBoundaries([NotNull] IIdentity identity)
        {
            IEnumerable<ITrustBoundary> result = null;

            if (identity is IDataFlow dataFlow)
            {
                var source = dataFlow.Source;
                var target = dataFlow.Target;
                if (source != null && target != null)
                {
                    var sourceParents = GetParents(source).ToArray();
                    var targetParents = GetParents(target).ToArray();
                    result = sourceParents.Except(targetParents).Union(targetParents.Except(sourceParents));
                }
            }

            return result;
        }

        protected IEnumerable<ITrustBoundary> GetEnteringTrustBoundaries([NotNull] IIdentity identity)
        {
            IEnumerable<ITrustBoundary> result = null;

            if (identity is IDataFlow dataFlow)
            {
                var source = dataFlow.Source;
                var target = dataFlow.Target;
                if (source != null && target != null)
                {
                    var sourceParents = GetParents(source).ToArray();
                    var targetParents = GetParents(target).ToArray();
                    result = targetParents.Except(sourceParents);
                }
            }

            return result;
        }

        protected IEnumerable<ITrustBoundary> GetExitingTrustBoundaries([NotNull] IIdentity identity)
        {
            IEnumerable<ITrustBoundary> result = null;

            if (identity is IDataFlow dataFlow)
            {
                var source = dataFlow.Source;
                var target = dataFlow.Target;
                if (source != null && target != null)
                {
                    var sourceParents = GetParents(source).ToArray();
                    var targetParents = GetParents(target).ToArray();
                    result = sourceParents.Except(targetParents);
                }
            }

            return result;
        }

        private IEnumerable<ITrustBoundary> GetParents([NotNull] IEntity entity)
        {
            List<ITrustBoundary> result = new List<ITrustBoundary>();

            if (entity.Parent != null)
            {
                if (entity.Parent is ITrustBoundary trustBoundary)
                    result.Add(trustBoundary);

                GetParentsRecursively(entity.Parent, result);
            }

            return result;
        }

        private void GetParentsRecursively(IGroup group, [NotNull] List<ITrustBoundary> parents)
        {
            if (group is IGroupElement groupElement)
            {
                if (groupElement.Parent is ITrustBoundary trustBoundary)
                    parents.Add(trustBoundary);

                GetParentsRecursively(groupElement.Parent, parents);
            }
        }
    }
}
