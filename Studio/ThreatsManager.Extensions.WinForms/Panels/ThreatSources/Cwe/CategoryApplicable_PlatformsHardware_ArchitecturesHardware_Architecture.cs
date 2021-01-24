// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 4.2.0.44
//  </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#pragma warning disable
namespace ThreatsManager.Extensions.Panels.ThreatSources.Cwe
{
    /// <summary>
/// This element identifies a single
/// Hardware_Architecture on which this entry may exist
/// and with what frequency on the specified
/// architecture.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType=true)]
public partial class CategoryApplicable_PlatformsHardware_ArchitecturesHardware_Architecture
{
    #region Private fields
    private CategoryApplicable_PlatformsHardware_ArchitecturesHardware_ArchitectureHardware_Architecture_Name _hardware_Architecture_Name;
    private Frequency_Type _prevalence;
    #endregion
    
    /// <summary>
    /// This subelement identifies
    /// architectures on which this weakness is likely to
    /// exist.
    /// </summary>
    [XmlAttribute]
    public CategoryApplicable_PlatformsHardware_ArchitecturesHardware_ArchitectureHardware_Architecture_Name Hardware_Architecture_Name
    {
        get
        {
            return _hardware_Architecture_Name;
        }
        set
        {
            _hardware_Architecture_Name = value;
        }
    }
    
    /// <summary>
    /// This attribute identifies the
    /// prevalence with which this weakness may occur
    /// within code that runs on the specified hardware
    /// architecture.
    /// </summary>
    [XmlAttribute]
    public Frequency_Type Prevalence
    {
        get
        {
            return _prevalence;
        }
        set
        {
            _prevalence = value;
        }
    }
}
}
#pragma warning restore
