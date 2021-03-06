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
/// Used to indicate how a condition should be applied to a list of values.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[XmlType(Namespace="http://cybox.mitre.org/common-2")]
public enum ConditionApplicationEnum
{
    /// <summary>
    /// Indicates that a pattern holds if the given condition can be successfully applied to any of the field values.
    /// </summary>
    ANY,
    /// <summary>
    /// Indicates that a pattern holds only if the given condition can be successfully applied to all of the field values.
    /// </summary>
    ALL,
    /// <summary>
    /// Indicates that a pattern holds only if the given condition can be successfully applied to none of the field values.
    /// </summary>
    NONE,
}
}
#pragma warning restore
