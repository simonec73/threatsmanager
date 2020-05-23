# SampleWinFormExtensions

Sample project to show Extensions that are designed for WinForms.

## Dependencies

The project relies on Nuget package "ThreatsManager.Utilities.WinForms", which loads all the requirements and creates file ExtensionLibrary.cs, which marks is as an Extensions Container.

## Implemented Extensions

The Extentions Container implements the following use cases, which are discussed then in the following chapters:

- Azure DevOps: a panel showing updates for Mitigations, coming from a fictious project on Azure DevOps.
- Definition: a container for common definitions, to be used for example to explain acronyms.
- Validate: a very simple quality validation interface, showing how to use advanced controls like Graphs inside a Panel.

### Azure DevOps

Follow the list of Extensions implemented to provide the experience for this scenario.

|Class Name   |Extension Type  |Description       |
|-------------|----------------|------------------|
|AzureDevOpsPanelFactory|Panel Factory|Panel Factory, having the responsibility of creating instances of AzureDevOpsPanel class.<br>Those classes provide the user interface for this functionality.|

### Definition

Follow the list of Extensions implemented to provide the experience for this scenario.

|Class Name   |Extension Type  |Description       |
|-------------|----------------|------------------|
|DefinitionsPanelFactory|Panel Factory|Panel Factory, having the responsibility of creating instances of DefinitionsPanel class.<br>Those classes provide the user interface for this functionality.
|DefinitionsSchemaInitializer|Initializer|Initialization of the Threat Model with some initial values for the Definition functionality.|
|ExtensionInitializer|IExtensionInitializer|An initializer used to introduce the object that defines the container for definitions, DefinitionContainer, so that it can be deserialized.|

The project includes also the following notable classes:

- DefinitionContainer: a class that can be serialized as Json, used to store the data.
- DefinitionsPropertySchemaManager: a class to manage a new Property Schema designed to store the data required by the Definition functionality. It defines a Json Property used to store the DefinitionContainer.
- DefinitionPanel: the main UI for this functionality.

### Validate

Follow the list of Extensions implemented to provide the experience for this scenario.

|Class Name   |Extension Type  |Description       |
|-------------|----------------|------------------|
|ValidatePanelFactory|Panel Factory|Panel Factory, having the responsibility of creating instances of ValidatePanel class.<br>Those classes provide the user interface for this functionality.|
