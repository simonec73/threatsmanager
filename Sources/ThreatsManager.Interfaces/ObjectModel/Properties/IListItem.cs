namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Interface that represents a List Item that is managed by the various types of Property Lists.
    /// </summary>
    public interface IListItem
    {
        /// <summary>
        /// Identifier of the List Item.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Label to be shown when the List Item is to be displayed.
        /// </summary>
        /// <remarks>If the Label is missing, the Id will be used in its wake.</remarks>
        string Label { get; set; }

        /// <summary>
        /// Description of the List Item.
        /// </summary>
        string Description { get; set; }
    }
}