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
/// The InternalStringsType contains a single internal string instance for this internationalization setting instance.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace="http://cybox.mitre.org/common-2")]
public partial class InternalStringsType
{
    #region Private fields
    private string _key;
    private string _content;
    #endregion
    
    /// <summary>
    /// This field contains the actual key of this internal string instance.
    /// </summary>
    public string Key
    {
        get => _key;
        set => _key = value;
    }
    
    /// <summary>
    /// This field contains the actual content of this internal string instance.
    /// </summary>
    public string Content
    {
        get => _content;
        set => _content = value;
    }
}
}
#pragma warning restore
