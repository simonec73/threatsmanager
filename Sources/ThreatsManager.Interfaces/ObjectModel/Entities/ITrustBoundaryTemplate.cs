namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Template for Trust Boundaries.
    /// </summary>
    public interface ITrustBoundaryTemplate : IItemTemplate
    {
        /// <summary>
        /// Create a new Trust Boundary based on the Template.
        /// </summary>
        /// <param name="name">Name of the new Trust Boundary.</param>
        /// <returns>New Trust Boundary created from the Template.</returns>
        ITrustBoundary CreateTrustBoundary(string name);

        /// <summary>
        /// Apply the Template to an existing Trust Boundary.
        /// </summary>
        /// <param name="boundary">Trust Boundary which needs to receive the Template.</param>
        /// <remarks>Applies all the properties to the Trust Boundary and changes its Template to point to the new one.</remarks>
        void ApplyTo(ITrustBoundary boundary);
 
        /// <summary>
        /// Creates a duplicate of the current Template and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Template.</returns>
        ITrustBoundaryTemplate Clone(ITrustBoundaryTemplatesContainer container);
    }
}