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
/// Possible values for representing time precision.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[XmlType(Namespace="http://cybox.mitre.org/common-2")]
public enum TimePrecisionEnum
{
    /// <summary>
    /// Time is precise to the given hour.
    /// </summary>
    hour,
    /// <summary>
    /// Time is precise to the given minute.
    /// </summary>
    minute,
    /// <summary>
    /// Time is precise to the given second (including fractional seconds).
    /// </summary>
    second,
}
}
#pragma warning restore
