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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType=true)]
public partial class Content_HistoryPrevious_Entry_Name
{
    #region Private fields
    private DateTime _name_Change_Date;
    private string _value;
    #endregion
    
    [XmlAttribute(DataType="date")]
    public DateTime Name_Change_Date
    {
        get
        {
            return _name_Change_Date;
        }
        set
        {
            _name_Change_Date = value;
        }
    }
    
    [XmlText]
    public string Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
        }
    }
}
}
#pragma warning restore
