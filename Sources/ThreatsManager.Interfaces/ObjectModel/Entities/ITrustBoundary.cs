namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface implemented by Trust Boundaries.
    /// </summary>
    public interface ITrustBoundary : IGroup, IGroupElement
    {
        /// <summary>
        /// Template used to generate the Trust Boundary.
        /// </summary>
        /// <remarks>It returns null if there is no known Template which generated the Trust Boundary.</remarks>
        ITrustBoundaryTemplate Template { get; }

        /// <summary>
        /// Disassociate the Trust Boundary from the underlying Template.
        /// </summary>
        void ResetTemplate();
    }
}