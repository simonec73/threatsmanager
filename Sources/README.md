# The Sources

This section includes the code for the Threats Manager Platform's Engine. The code published here is designed to allow the implementation of solutions based on .NET Framework to create applications on multiple platforms, including WinForms and ASP.NET.

## How to Build

The code has been thought to be opened with Visual Studio 2019. The Community Edition would be enough.
You need also to install the following prerequisites:

- PostSharp Tools for Visual Studio **v6.6.7** (<https://www.postsharp.net/downloads/postsharp-6.6/v6.6.7>) with any License. The Community License can be obtained from <https://www.postsharp.net/download>.

### PostSharp

PostSharp is a powerful tool that has allowed to write less code and improve the quality of what has been built. Threats Manager Platform makes heavy use of PostSharp in various places: you may find aspects developed to implement common code in the Engine and in the Utilities library.
Threats Manager Platform makes use of a specific version of PostSharp. It will be regularly updated, but some releases will be skipped: you need to keep PostSharp Tools aligned with the version of PostSharp Nuget packages used in the solution. The version listed in **How to Build** is the current version adopted.
> You need to install any License. PostSharp will apply the Ultimate license to code in ThreatsManager namespace automatically.

## License

Threats Manager Platform code is provided under the MIT license.
The only exception is assembly ThreatsManager.Icons.dll, which contains icons part of Incors Gmbh IconExperience I-Collection v2.0 (<http://incors.com/info/>). Those icons are licensed by Incors and cannot be used outside of Threats Manager Platform. Further information in the **Resources** folder.
