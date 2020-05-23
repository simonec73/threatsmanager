# SampleExtensions

Sample project to show Extensions that are relatively platform-independent. In fact, Threats Manager Platform has been developed with .NET Frameword 4.7.2, which somewhat limits the platforms where it could be applicable.

## Dependencies

The project relies on Nuget package "ThreatsManager.Utilities", which loads all the requirements and creates file ExtensionLibrary.cs, which marks is as an Extensions Container.

## Implemented Extensions

The implemented extensions, are:

|Class Name   |Extension Type  |Description       |
|-------------|----------------|------------------|
|ConvertNameToUpperAction|Context Aware Action|A simple extension that creates a new action in the context menu for Entities (External Interactor, Processes and Data Stores), to convert their name to upper case.|
|ScanAzureSubscription|Main Ribbon Button|Creates a button in the Import ribbon, to simulate scanning of some Azure subscription and creation of a bunch of some Entities in the current Threat Model.|
