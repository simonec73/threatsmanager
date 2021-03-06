// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 4.2.0.44
//  </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#pragma warning disable
namespace ThreatsManager.Extensions.Panels.ThreatSources.Capec
{
    /// <summary>
/// PlatformSpecificationType is a modularized data type intended for providing a consistent approach to uniquely specifying the identity of a specific platform.
/// </summary>
/// <summary>
/// In addition to capturing basic information, this type is intended to be extended to enable the structured description of a platform instance using the XML Schema extension feature. The CybOX default extension uses the Common Platform Enumeration (CPE) Applicability Language schema to do so. The extension that defines this is captured in the CPE23PlatformSpecificationType in the http://cybox.mitre.org/extensions/platform#CPE2.3-1 namespace. This type is defined in the extensions/platform/cpe2.3.xsd file.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace="http://cybox.mitre.org/common-2")]
public partial class PlatformSpecificationType
{
    #region Private fields
    private StructuredTextType _description;
    private List<PlatformIdentifierType> _identifier;
    #endregion
    
    /// <summary>
    /// PlatformSpecificationType class constructor
    /// </summary>
    public PlatformSpecificationType()
    {
        _identifier = new List<PlatformIdentifierType>();
        _description = new StructuredTextType();
    }
    
    /// <summary>
    /// A prose description of the indicated platform.
    /// </summary>
    public StructuredTextType Description
    {
        get => _description;
        set => _description = value;
    }
    
    /// <summary>
    /// Indicates a pre-defined name for the given platform using some naming scheme. For example, one could provide a CPE (Common Platform Enumeration) name using the CPE naming format.
    /// </summary>
    [XmlElement("Identifier")]
    public List<PlatformIdentifierType> Identifier
    {
        get => _identifier;
        set => _identifier = value;
    }
}
}
#pragma warning restore
