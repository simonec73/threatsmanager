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
/// The Common_Platform subelement
/// identifies a single platform that is associated with
/// this weakness. Its only child, CPE_ID is required
/// and identifies the related CPE entry. More than one
/// Common_Platform_Reference element can exist, but
/// they must all be contained within a single
/// Common_Platform_References element.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType=true)]
public partial class CategoryApplicable_PlatformsCommon_Platform_Reference
{
    #region Private fields
    private string _cPE_ID;
    #endregion
    
    /// <summary>
    /// The CPE_ID stores the value for
    /// the related CPE entry identifier as a string. Only
    /// one CPE_ID element can exist for each
    /// Common_Platform element.
    /// </summary>
    public string CPE_ID
    {
        get
        {
            return _cPE_ID;
        }
        set
        {
            _cPE_ID = value;
        }
    }
}
}
#pragma warning restore
