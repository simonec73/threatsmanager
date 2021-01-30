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
/// The EaseOfObfuscationEnum is a (non-exhaustive) enumeration of simple characterizations of how easy it would be for an attacker to obfuscate the observability of this Observable.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[XmlType(Namespace="http://cybox.mitre.org/cybox-2")]
public enum EaseOfObfuscationEnum
{
    /// <summary>
    /// Specifies that this observable is very easy to obfuscate and hide.
    /// </summary>
    High,
    /// <summary>
    /// Specifies that this observable is somewhat easy to obfuscate and hide.
    /// </summary>
    Medium,
    /// <summary>
    /// Specifies that this observable is not very easy to obfuscate and hide.
    /// </summary>
    Low,
}
}
#pragma warning restore