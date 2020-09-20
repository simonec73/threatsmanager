# Threats Manager Platform Samples

Threats Manager Platform is first and foremost an extensible platform for Threats Management. This is achieved through two main means.

- It is possible to create components called Extensions, to provide additional features and extend the behavior of the solution. The user can create a personalized experience by selecting the Extensions that are most aligned with its needs.
- The Engine can be re-hosted. This allows to create new applications and extend the ecosystem, to support different ways to manage the Threat Model or to interact with different environments.

This section includes examples that can be used to learn how to achieve both goals.

## The Extensions

Threats Manager Platform supports different types of Extensions. While only some of them are covered here, more will be in the near future. The following table summarizes the situation, listing all the different types of supported Extensions, listed alphabetically.

It is important to note that the following table uses the following terminology:

- Extension is a class which implements one of the interfaces listed below, and that is marked with attribute ExtensionAttribute.
- Extension container, which is an Assembly marked with Attribute ExtensionContainerAttribute and that contains one or more Extensions.

|Extension Name    |Main Interface     |Description                             |Reference Sample         |
|------------------|-------------------|----------------------------------------|-------------------------|
|Context Aware Action|IContextAwareAction|Context Aware Actions are used to show context actions.|SampleExtensions|
|DevOps Connector  |IDevOpsConnector   |Connector to a DevOps/Bug Tracking software like Azure DevOps, GitHub or Jira.<br><br>**Important**<br>This feature is under active development and therefore the interface is subject to change.|  |
|Extension Initializer|IExtensionInitializer|Initializer executed when the Extension is loaded.<br>They are typically used to allow deserialization of Json properties.|SampleWinFormExtensions|
|Initializer       |IInitializer       |Initializers are used to provision initial data to new Threat Models.<br>This extension is complementary to Post-Load Threat Model Processor.|SampleWinFormExtensions|
|List Provider     |IListProviderExtension|List Providers are used by some types of Property Types (List and Multi List) to provide values from which the user is supposed to select from.|  |
|Main Ribbon Button|IMainRibbonExtension|The Main Ribbon Buttons define features that can be executed by clicking a button in the main interface.<br>Even if the name refers to a specific type of interface, there is no real need to rely on a Ribbon, provided that the Host application is able to surface such Extensions in some way.| |
|Package Manager   |IPackageManager    |Package Managers are used to support different storages.<br><br>**Important**<br>Package Managers are subject to change.|  |
|Panel Factory     |IPanelFactory      |Panel Factories create Panels that represent interfaces in WinForm-based clients, like Threats Manager Studio.<br>Different types of interfaces would require different Extensions.|SampleWinFormExtensions|
|Post-Load Threat Model Processor|IPostLoadProcessor|Extension executing additional actions as soon as the Threat Model has been loaded.<br>This extension is complementary to Initializers.|  |
|Property Schemas Extractor|IPropertySchemasExtractor|Property Schemas Extractors are used to get references to the Property Schemas used in the various Json Properties defined in some Extensions Container.<br>An example is represented by the AutoThreatGen extension container, which stores rules associated to Threat Types and Mitigations. Those rules may refer other property in Property Schemas, which are identified with this Extension.<br>This extension is complementary to Property Schema Updaters.|  |
|Property Schemas Updater|IPropertySchemasUpdater|Property Schemas Updaters are used to propagate changes to the Property Schema to Json Properties where those Schemas are used.<br>An example is represented by the AutoThreatGen extension container, which stores rules associated to Threat Types and Mitigations and as a result need to receive changes to those Property Schemas.<br>This extension is complementary to Property Schemas Extractors.|  |
|Quality Analyzer  |IQualityAnalyzer   |Quality Analyzers are used by the Quality Dashboard in the Quality Extension to evaluate the Threat Model.|  |
|Residual Risk Estimator|IResidualRiskEstimator|Extension used to estimate the Residual Risk and to evaluate the impact of Mitigations.|  |
|Settings Panel Provider|ISettingsPanelProvider|Interface used to extend the configuration interface with specific pages.<br><br>**Important**<br>This feature is under active development and therefore the interface is subject to change.|  |
|Status Info Provider|IStatusInfoProviderExtension|Status Info Providers are used to show counters and other information in the Status Bar.|  |

## The Engine

The Samples include the following project which shows how to rehost the Engine.

|Sample                   |Project Type  |
|-------------------------|--------------|
|SimpleThreatModelAnalyzer|Console       |

Tool TmtImport in https://github.com/simonec73/threatsmanager/tree/master/Tools/TmtImport is another example of Engine rehosting.