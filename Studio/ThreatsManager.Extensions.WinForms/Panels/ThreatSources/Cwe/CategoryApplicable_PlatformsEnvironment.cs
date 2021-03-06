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
/// Environment in which this entry may exist and with
/// what frequency in the specified environment.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType=true)]
public partial class CategoryApplicable_PlatformsEnvironment
{
    #region Private fields
    private string _environment_Name;
    private Frequency_Type _prevalence;
    #endregion
    
    /// <summary>
    /// This attribute is the name of
    /// the Environment we are identifying.
    /// </summary>
    [XmlAttribute]
    public string Environment_Name
    {
        get
        {
            return _environment_Name;
        }
        set
        {
            _environment_Name = value;
        }
    }
    
    /// <summary>
    /// This attribute identifies the
    /// prevalence with which this weakness may occur
    /// within code that runs on the specified
    /// environment.
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
