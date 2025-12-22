# Threats Manager Platform Core libraries and SDK

## Definition

Threats Manager Platform is a Threat Modeling solution that has been designed to be rehostable, extendable and flexible.
It has been designed and implemented by a group of experienced Threat Modelers, to satisfy the needs of their day by day job, and it has been evolved over the time to cover additional scenarios.
Threats Manager Platform is the reference implementation of the Threat Modeling vNext vision, discussed in <https://simoneonsecurity.com>.

The code included in this Repo is to be also available as part of a complete Threat Modeling tool, called Threats Manager Studio, which implements the Windows Desktop experience. That tool along with a number of Extensions is available from <https://threatsmanager.com> and here.
Threats Manager Studio and the Windows Desktop Extensions are also available as Open Source. You may not be able to build them, though, because they are based on various commercial components which cannot be delivered as part of this distribution. Still, Threats Manager Studio and its Extensions are freely downloadable and usable in all contexts, under a very permissive license. 
Threats Manager Studio represents both a reference implementation of the Threats Manager Platform and a tool you can adopt for your Threat Modeling activities.

## Installation

The latest version of Threats Manager Studio (TMS), v2.2.8, can be downloaded from [here](https://downloads.threatsmanager.com/latest/ThreatsManagerStudio_2.2.8.0.exe).
This version of TMS is based on [Threats Manager Platform](https://github.com/simonec73/threatsmanager) (TMP) v2.2.8.
The SHA256 hash of the distribution file is 85500AC977EE40B5E1014E60CBE92346C05AA2180AC09FB9F29705B09931C20E.
Please refer to the [What’s New](https://downloads.threatsmanager.com/latest/What's New.txt) document for details on fixes and improvements over the previous version.

If you need an introduction to TMS or to the Extensions, please refer to the [Learning](https://threatsmanager.com/training/) pages.
Guidance on installing and configuring TMS and the Extensions is available [here](https://threatsmanager.com/training/intro/start-here/install-and-configure/).

The SBOM for the latest version of Threats Manager Platform is available [here](https://downloads.threatsmanager.com/latest/TMP_SBOM_2.2.8.0.zip).
The SBOM for the latest version of Threats Manager Studio is available [here](https://downloads.threatsmanager.com/latest/TMS_SBOM_2.2.8.0.zip).

**Requirements**
– At least 4 GBytes of RAM. 8 GBytes of RAM are recommended.
– A screen with FHD resolution (1920×1080) or better. HD resolution (1280×720) may be workable with some limitations.
– A recent x86 CPU. Intel Core i5 6th generation or better is recommended.
– Windows 10 22H2, or Windows 11 22H2 or later.
– Internet connectivity.

The following requirement will be automatically installed if you do not have it:
– [.Net Framework 4.8.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net481).

### Portable distribution

Threats Manager Studio is also available as a portable distribution which you can download from [here](https://downloads.threatsmanager.com/latest/TMS_Portable_2.2.8.0.zip).
It contains most of the same capabilities in a package that can be extracted anywhere and executed without installing anything.
The main differences between the installed portable and the normal distributions are:
– The portable distribution does not have integrated guidance.
– The portable distribution includes the following extension libraries: Auto Threat Gen, MS TMT Import, and Quality. The other extension libraries are supported but must be installed and configured.
– The same requirements for the normal distribution apply, but Internet connectivity is not required for the portable distribution.
The SHA256 hash of the distribution file is 4A74E4615979622044199113F65B3BD59CE41558CD214ADB186D2FF902A5F5D8.

**IMPORTANT**
Remember to Unblock the ZIP file before extracting its content. You can do that from its Properties. You can also use the PowerShell command [Unblock-File](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/unblock-file?view=powershell-7.3).

### Most typical Extensions

This section contains a list of the most recommended Extensions, which are typically required. You can find additional Extensions below.

> **IMPORTANT**
> Please note that Extensions can be loaded by TMS only if they support the version of TMP used by TMS.
> For example, if TMS has been built with TMP v2.4.2, then an Extension supporting TMP in the version range 2.4.0 to 2.4.99 would be loaded and executed. On the contrary, an Extension supporting TMP in the range of 2.3.0 to 2.3.99 would not be loaded.
> Initial versions of the Extensions support a specific version of TMP only.

| Extension                   | Supported TMP | Requirements | SHA256                                                       | Links                                                        |
| --------------------------- | ------------- | ------------ | ------------------------------------------------------------ | ------------------------------------------------------------ |
| Automatic Threat Generation | 2.2.8         | None         | 6378CE6B865F5678F412D402E53101343DB076F8AB2AA5F8C1352E6F676499B8 | [Download](https://downloads.threatsmanager.com/latest/TMS_AutoThreatGen_2.2.8.0.msi) [Documentation](https://threatsmanager.com/training/advanced/extensions/auto-threat-generation/) |
| Quality                     | 2.2.8         | None         | A341E2F8ED8E996C401541A3CD667B764B6DF67EFCB7D83EEEDAC2B5F9B6C1F2 | [Download](https://downloads.threatsmanager.com/latest/TMS_Quality_2.2.8.0.msi) [Documentation](https://threatsmanager.com/training/advanced/extensions/quality/) |

### Additional Extensions

> **IMPORTANT**
> Please note that Extensions can be loaded by TMS only if they support the version of TMP used by TMS.
> For example, if TMS has been built with TMP v2.4.2, then an Extension supporting TMP in the version range 2.4.0 to 2.4.99 would be loaded and executed. On the contrary, an Extension supporting TMP in the range of 2.3.0 to 2.3.99 would not be loaded.
> Initial versions of the Extensions support a specific version of TMP only.

| Extension                      | Supported TMP | Requirements                                   | SHA256                                                       | Links                                                        |
| ------------------------------ | ------------- | ---------------------------------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| DevOps                         | 2.2.8         | Internet connectivity for some functionalities | 7F2460939D58E0FDEF91E3B9A9D4D9340EABF4014FAF37F0C244193DE94893F0 | [Download](https://downloads.threatsmanager.com/latest/TMS_DevOps_2.2.8.0.msi) [Documentation](https://threatsmanager.com/training/advanced/extensions/devops/) |
| MS Threat Modeling Tool Import | 2.2.8         | None                                           | 6D9B8D255F255BFC163BCA03C49A9487DA31C4E7FF591AA2234B49D755CC132F | [Download](https://downloads.threatsmanager.com/latest/TMS_MsTmt_2.2.8.0.msi) [Documentation](https://threatsmanager.com/training/advanced/extensions/tmt-importer/) |

### Additional Tools

TMS allows creating Microsoft Word reports from curated [DOCX or DOTX Templates](https://threatsmanager.com/training/advanced/reporting/word-template/).
To create those templates, it is possible to use a Microsoft Word Add-In, which can be installed from [Download](https://downloads.threatsmanager.com/latest/TMPlatform_ReportingAddIn.msi).
The SHA256 hash of the installation file is 2104F2FC5A44BA55553F80AFAFA04643665F3EAA08297DD9FF723ABBFF91545E.

**Requirements**
– The same as Microsoft Word.

## Important

The main author of Threats Manager Platform, Simone Curzi, and most contributors are employees in Microsoft. 
Nevertheless, Threats Manager Platform is NOT a Microsoft product. It is **NOT** endorsed, supported or guaranteed by Microsoft in any way.
