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
/// The FrequencyType is a type representing the specification of a frequency for a given action or event.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace="http://cybox.mitre.org/cybox-2")]
public partial class FrequencyType
{
    #region Private fields
    private float _rate;
    private string _units;
    private string _scale;
    private TrendEnum _trend;
    #endregion
    
    /// <summary>
    /// This field specifies the rate for this defined frequency.
    /// </summary>
    [XmlAttribute]
    public float rate
    {
        get => _rate;
        set => _rate = value;
    }
    
    /// <summary>
    /// This field specifies the units for this defined frequency.
    /// </summary>
    [XmlAttribute]
    public string units
    {
        get => _units;
        set => _units = value;
    }
    
    /// <summary>
    /// This field specifies the time scale for this defined frequency.
    /// </summary>
    [XmlAttribute]
    public string scale
    {
        get => _scale;
        set => _scale = value;
    }
    
    /// <summary>
    /// This field is optional and conveys a targeted observation pattern of the nature of any trend in the frequency of the associated event or action. This field would be leveraged within an event or action pattern observable triggering on the matching of a specified trend in the frequency of an event or action.
    /// </summary>
    [XmlAttribute]
    public TrendEnum trend
    {
        get => _trend;
        set => _trend = value;
    }
}
}
#pragma warning restore
