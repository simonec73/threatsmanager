namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Enumeration of the available Ribbons.
    /// </summary>
    public enum Ribbon
    {
        /// <summary>
        /// The main Ribbon.
        /// </summary>
        Home,

        /// <summary>
        /// The Ribbon used to collect the actions to insert objects and entities in the Model.
        /// </summary>
        Insert,

        /// <summary>
        /// The Ribbon used to perform Analysis of the Model.
        /// </summary>
        Analyze,

        /// <summary>
        /// The Ribbon used to collect the additional Views.
        /// </summary>
        View,

        /// <summary>
        /// The Ribbon collecting all the actions to Export the Model.
        /// </summary>
        Export,

        /// <summary>
        /// The Ribbon collecting all the actions to Import objects into the Model.
        /// </summary>
        Import,

        /// <summary>
        /// The Ribbon collecting all Extensions to integrate with external services,
        /// which would not fit the other Ribbons.
        /// </summary>
        Integrate,

        /// <summary>
        /// The Ribbon used to perform Review activities.
        /// </summary>
        Review,

        /// <summary>
        /// The Ribbon hosting configuration extensions.
        /// </summary>
        Configure,

        /// <summary>
        /// The Ribbon implementing the Help system.
        /// </summary>
        Help,
    }
}