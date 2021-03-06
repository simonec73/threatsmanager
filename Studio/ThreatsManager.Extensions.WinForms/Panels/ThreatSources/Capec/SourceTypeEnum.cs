// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 4.2.0.44
//  </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.Xml.Serialization;

#pragma warning disable
namespace ThreatsManager.Extensions.Panels.ThreatSources.Capec
{
    /// <summary>
/// The SourceTypeEnum is a (non-exhaustive) enumeration of cyber observation source types.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[XmlType(Namespace="http://cybox.mitre.org/common-2")]
public enum SourceTypeEnum
{
    /// <summary>
    /// Describes a cyber observation made using various tools, such as scanners, firewalls, gateways, protection systems, and detection systems. See ToolTypeEnum for a more complete list of tools that CybOX supports.
    /// </summary>
    Tool,
    /// <summary>
    /// Describes a cyber observation made from analysis methods, such as Static and Dynamic methods. See AnalysisMethodTypeEnum for a more complete list of methods that CybOX supports.
    /// </summary>
    Analysis,
    /// <summary>
    /// Describes a cyber observation made using other information sources, such as logs, Device Driver APIs, and TPM output data. See InformationSourceTypeEnum for a more complete list of information sources that CybOX supports.
    /// </summary>
    [XmlEnum("Information Source")]
    InformationSource,
}
}
#pragma warning restore
