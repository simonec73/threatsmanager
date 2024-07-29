namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    internal enum ThreatsPolicy
    {
        Undefined,
        Skip,
        ThreatModel,
        Processes,
        DataStores,
        Full,
        ProcessesFromEI,
        DataStoresFromEI,
        FullFromEI,
        AllProcesses,
        AllDataStores,
        AllProcessesFromEI,
        AllDataStoresFromEI,
        AllFromEI,
        CrossTB,
        FullCrossTB
    }
}
