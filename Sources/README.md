# The Sources

This section includes the code for the Threats Manager Platform's Engine. The code published here is designed to allow the implementation of solutions based on .NET Framework to create applications on multiple platforms, including WinForms and ASP.NET.
Those libraries are often referred as the "Core libraries" or simply as "Core", in the context of Threats Manager Platform.

Folder Extensions contains also the code for logic implemented for some Extensions. The logic stored here is designed to be adopted for building solutions on multiple platforms, and represents the main logic implemented for the various Extensions built as part of the Windows Desktop experience of the Threats Manager Platform, which will be published soon.
The said Windows Desktop experience will not available as Open Source, because it is based on various commercial components, but it will be freely downloadable and usable in all contexts, under a very permissive license.

** Now it supports both .NET 4.7.2 and .NET Core 3.1! **

## How to Build

The code has been thought to be opened with Visual Studio 2019. The Community Edition would be enough.
You need also to install the following prerequisites:

- PostSharp Tools for Visual Studio **v6.6.13** (<https://www.postsharp.net/downloads/postsharp-6.6/v6.6.13>) with any License. The Community License can be obtained from <https://www.postsharp.net/download>.

### PostSharp

PostSharp is a powerful tool that has allowed to write less code and improve the quality of what has been built. Threats Manager Platform makes heavy use of PostSharp in various places: you may find aspects developed to implement common code in the Engine and in the Utilities library.
Threats Manager Platform makes use of a specific version of PostSharp. It will be regularly updated, but some releases will be skipped: you need to keep PostSharp Tools aligned with the version of PostSharp Nuget packages used in the solution. The version listed in **How to Build** is the current version adopted.
> You need to install any License. PostSharp will apply the Ultimate license to code in ThreatsManager namespace automatically.

## License

Threats Manager Platform code is provided under the MIT license.
The only exception is assembly ThreatsManager.Icons.dll, which contains icons part of Incors Gmbh IconExperience I-Collection v2.0 (<http://incors.com/info/>). Those icons are licensed by Incors and cannot be used outside of Threats Manager Platform. Further information in the **Resources** folder.

## How to use

The Threats Manager Platform Core libraries are all available as NuGet packages.

- ThreatsManager.Interfaces (<https://www.nuget.org/packages/ThreatsManager.Interfaces/>)
- ThreatsManager.Utilities (<https://www.nuget.org/packages/ThreatsManager.Utilities/>)
- ThreatsManager.Engine (<https://www.nuget.org/packages/ThreatsManager.Engine/>)

There is also a fourth NuGet package, for additional utilities to cover WinForms platform: ThreatsManager.Utilities.WinForms (<https://www.nuget.org/packages/ThreatsManager.Utilities.WinForms/>). This library is not part of this Repo.

## The available Extensions

The sources include the sources for some Extensions. The code collected here does not provide any UI-specific code and is intended to be used for building other tools and Extensions.

|Extension                          |Description   |
|-----------------------------------|--------------|
|ThreatsManager.AutoThreatGeneration|Library to support automatic Threat Generation and Mitigation association.|
|ThreatsManager.MsTmt               |Library to support conversion of Microsoft Threat Modeling Tool documents and Templates to the Threats Manager Platform format|

