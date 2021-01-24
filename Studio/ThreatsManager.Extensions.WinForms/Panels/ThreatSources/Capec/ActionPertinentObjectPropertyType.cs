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
namespace ThreatsManager.Extensions.Panels.ThreatSources.Capec
{
    /// <summary>
/// The ActionPertinentObjectPropertyType identifies one of the Properties of an Object that specifically pertinent to an Action.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace="http://cybox.mitre.org/cybox-2")]
public partial class ActionPertinentObjectPropertyType
{
    #region Private fields
    private string _name;
    private string _xpath;
    #endregion
    
    /// <summary>
    /// The name field specifies the field name for the pertinent Object Property.
    /// </summary>
    [XmlAttribute]
    public string name
    {
        get => _name;
        set => _name = value;
    }
    
    /// <summary>
    /// The xpath field specifies the XPath 1.0 expression identifying the pertinent property within the Properties schema for this object type.
    /// </summary>
    [XmlAttribute]
    public string xpath
    {
        get => _xpath;
        set => _xpath = value;
    }
}
}
#pragma warning restore
