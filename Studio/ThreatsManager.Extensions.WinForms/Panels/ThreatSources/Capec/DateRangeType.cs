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
/// The DateRangeType specifies a range of dates.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace="http://cybox.mitre.org/common-2")]
public partial class DateRangeType
{
    #region Private fields
    private DateWithPrecisionType _start_Date;
    private DateWithPrecisionType _end_Date;
    #endregion
    
    /// <summary>
    /// DateRangeType class constructor
    /// </summary>
    public DateRangeType()
    {
        _end_Date = new DateWithPrecisionType();
        _start_Date = new DateWithPrecisionType();
    }
    
    /// <summary>
    /// This field contains the start date for this contributor's involvement. In order to avoid ambiguity, it is strongly suggest that all timestamps in this field include a specification of the timezone if it is known.
    /// </summary>
    public DateWithPrecisionType Start_Date
    {
        get => _start_Date;
        set => _start_Date = value;
    }
    
    /// <summary>
    /// This field contains the end date for this contributor's involvement. In order to avoid ambiguity, it is strongly suggest that all timestamps in this field include a specification of the timezone if it is known.
    /// </summary>
    public DateWithPrecisionType End_Date
    {
        get => _end_Date;
        set => _end_Date = value;
    }
}
}
#pragma warning restore
