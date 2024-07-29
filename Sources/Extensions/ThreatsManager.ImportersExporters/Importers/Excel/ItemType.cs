using ThreatsManager.Interfaces;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    public enum ItemType
    {
        [UiHidden]
        Unknown,

        [EnumLabel("External Interactor")]
        ExternalInteractor,

        Process,

        [EnumLabel("Data Store")]
        DataStore,

        Flow,

        [EnumLabel("Trust Boundary")]
        TrustBoundary
    }
}