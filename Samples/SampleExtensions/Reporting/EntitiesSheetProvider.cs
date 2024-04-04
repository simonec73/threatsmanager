using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Extensions.Reporting;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace SampleExtensions.Reporting
{
    /// <summary>
    /// Summmary Sheet Providers add sheets to the Excel Summary.
    /// This example is designed to add a table with the list of all entities defined in the threat model.
    /// </summary>
    [Extension("14BF4591-C8C6-4971-A324-AC966ECFB389", "Entities Sheet Provider", 100, ExecutionMode.Business)]
    public class EntitiesSheetProvider : ISummarySheetProvider
    {
        /// <summary>
        /// The name of the sheet to be created in Excel.
        /// </summary>
        public string Name => "Entities";

        /// <summary>
        /// Method called to get the rows of the table.
        /// </summary>
        /// <param name="model">Threat Model containing the information used to generate the sheet.</param>
        /// <returns>The data to be used to create the sheet.</returns>
        /// <remarks>TMS checks that every row has the same number of items. If not, it does not create the sheet.
        /// The first row contains the header.</remarks>
        public IEnumerable<IEnumerable<string>> GetRows(IThreatModel model)
        {
            IEnumerable<IEnumerable<string>> result = null;

            // Let's first get the list of items to be created.
            // This list could be sorted as required.
            var entities = model?.Entities?.ToArray();
            
            if (entities?.Any() ?? false)
            {
                var table = new List<IEnumerable<string>>();

                // The first row contains the header.
                table.Add(new[] { "Name", "Entity Type", "Description" });

                // Enumeration of the entities.
                foreach ( var entity in entities)
                {
                    table.Add(new[] { entity.Name, entity.GetEntityType().GetEnumLabel(), entity.Description });
                }

                result = table;
            }

            return result;
        }
    }
}
