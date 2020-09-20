using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel
{
    /// <summary>
    /// Definition of what parts of the Threat Model need to be inserted in a duplicate. 
    /// </summary>
    public class DuplicationDefinition
    {
        /// <summary>
        /// If true, all Contributors will be included.
        /// </summary>
        public bool Contributors;

        /// <summary>
        /// If true, all Assumptions will be included.
        /// </summary>
        public bool Assumptions;

        /// <summary>
        /// If true, all Dependencies will be included.
        /// </summary>
        public bool Dependencies;

        /// <summary>
        /// If true, all Properties will be included.
        /// </summary>
        /// <remarks>If true, <see cref="Properties"/> will be ignored.</remarks>
        public bool AllProperties;

        /// <summary>
        /// List of the Properties to include.
        /// </summary>
        /// <remarks>If <see cref="AllProperties"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> Properties;

        /// <summary>
        /// If true, all Entities will be included.
        /// </summary>
        /// <remarks>If true, <see cref="Entities"/> will be ignored.</remarks>
        public bool AllEntities;

        /// <summary>
        /// List of the Entities to include.
        /// </summary>
        /// <remarks>If <see cref="AllEntities"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> Entities;

        /// <summary>
        /// If true, all Groups will be included.
        /// </summary>
        /// <remarks>If true, <see cref="Groups"/> will be ignored.</remarks>
        public bool AllGroups;

        /// <summary>
        /// List of the Groups to include.
        /// </summary>
        /// <remarks>If <see cref="AllGroups"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> Groups;

        /// <summary>
        /// If true, all Data Flows will be included.
        /// </summary>
        /// <remarks>If true, <see cref="DataFlows"/> will be ignored.</remarks>
        public bool AllDataFlows;

        /// <summary>
        /// List of the Entities to include.
        /// </summary>
        /// <remarks>If <see cref="AllDataFlows"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> DataFlows;

        /// <summary>
        /// If true, all Property Schemas will be included.
        /// </summary>
        /// <remarks>If true, <see cref="PropertySchemas"/> will be ignored.</remarks>
        public bool AllPropertySchemas;

        /// <summary>
        /// List of the Property Schemas to include.
        /// </summary>
        /// <remarks>If <see cref="AllDataFlows"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> PropertySchemas;

        /// <summary>
        /// If true, all Diagrams will be included.
        /// </summary>
        /// <remarks>If true, <see cref="Diagrams"/> will be ignored.</remarks>
        public bool AllDiagrams;

        /// <summary>
        /// List of the Diagrams to include.
        /// </summary>
        /// <remarks>If <see cref="AllDiagrams"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> Diagrams;

        /// <summary>
        /// If true, all Severities will be included.
        /// </summary>
        /// <remarks>If true, <see cref="Severities"/> will be ignored.</remarks>
        public bool AllSeverities;

        /// <summary>
        /// List of the Severities to include.
        /// </summary>
        /// <remarks>If <see cref="AllSeverities"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<int> Severities;

        /// <summary>
        /// If true, all Threat Types will be included.
        /// </summary>
        /// <remarks>If true, <see cref="ThreatTypes"/> will be ignored.</remarks>
        public bool AllThreatTypes;

        /// <summary>
        /// List of the Threat Types to include.
        /// </summary>
        /// <remarks>If <see cref="AllThreatTypes"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> ThreatTypes;

        /// <summary>
        /// If true, all Strengths will be included.
        /// </summary>
        /// <remarks>If true, <see cref="Strengths"/> will be ignored.</remarks>
        public bool AllStrengths;

        /// <summary>
        /// List of the Strengths to include.
        /// </summary>
        /// <remarks>If <see cref="AllStrengths"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<int> Strengths;

        /// <summary>
        /// If true, all Mitigations will be included.
        /// </summary>
        /// <remarks>If true, <see cref="Mitigations"/> will be ignored.</remarks>
        public bool AllMitigations;

        /// <summary>
        /// List of the Mitigations to include.
        /// </summary>
        /// <remarks>If <see cref="AllMitigations"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> Mitigations;

        /// <summary>
        /// If true, all Threat Actors will be included.
        /// </summary>
        /// <remarks>If true, <see cref="ThreatActors"/> will be ignored.</remarks>
        public bool AllThreatActors;

        /// <summary>
        /// List of the Threat Actors to include.
        /// </summary>
        /// <remarks>If <see cref="AllThreatActors"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> ThreatActors;

        /// <summary>
        /// If true, all Entity Templates will be included.
        /// </summary>
        /// <remarks>If true, <see cref="EntityTemplates"/> will be ignored.</remarks>
        public bool AllEntityTemplates;

        /// <summary>
        /// List of the Entity Templates to include.
        /// </summary>
        /// <remarks>If <see cref="AllEntityTemplates"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> EntityTemplates;

        /// <summary>
        /// If true, all Flow Templates will be included.
        /// </summary>
        /// <remarks>If true, <see cref="FlowTemplates"/> will be ignored.</remarks>
        public bool AllFlowTemplates;

        /// <summary>
        /// List of the Flow Templates to include.
        /// </summary>
        /// <remarks>If <see cref="AllFlowTemplates"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> FlowTemplates;

        /// <summary>
        /// If true, all Trust Boundary Templates will be included.
        /// </summary>
        /// <remarks>If true, <see cref="TrustBoundaryTemplates"/> will be ignored.</remarks>
        public bool AllTrustBoundaryTemplates;

        /// <summary>
        /// List of the Trust Boundary Templates to include.
        /// </summary>
        /// <remarks>If <see cref="AllTrustBoundaryTemplates"/> is true, then this list will be ignored.</remarks>
        public IEnumerable<Guid> TrustBoundaryTemplates;
    }
}
