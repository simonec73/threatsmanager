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
namespace ThreatsManager.Extensions.Panels.ThreatSources.Cwe
{
    /// <summary>
/// This element contains one or more
/// Operating_System subelements which each represent an OS in which
/// this weakness is likely to exist.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType=true)]
public partial class CategoryApplicable_PlatformsOperating_Systems
{
    #region Private fields
    private List<CategoryApplicable_PlatformsOperating_SystemsOperating_System> _operating_System;
    private List<CategoryApplicable_PlatformsOperating_SystemsOperating_System_Class> _operating_System_Class;
    #endregion
    
    /// <summary>
    /// CategoryApplicable_PlatformsOperating_Systems class constructor
    /// </summary>
    public CategoryApplicable_PlatformsOperating_Systems()
    {
        _operating_System_Class = new List<CategoryApplicable_PlatformsOperating_SystemsOperating_System_Class>();
        _operating_System = new List<CategoryApplicable_PlatformsOperating_SystemsOperating_System>();
    }
    
    /// <summary>
    /// This element identifies a single
    /// Operating_System in which this entry may exist and
    /// with what frequency on the specified OS.
    /// </summary>
    [XmlElement("Operating_System")]
    public List<CategoryApplicable_PlatformsOperating_SystemsOperating_System> Operating_System
    {
        get
        {
            return _operating_System;
        }
        set
        {
            _operating_System = value;
        }
    }
    
    /// <summary>
    /// This element identifies a single
    /// class of operating systems, specified in
    /// Operating_System_Class_Description, in which this
    /// entry may exist. Suggested values include: Linux,
    /// Windows, UNIX, BSD, and Mac OS.
    /// </summary>
    [XmlElement("Operating_System_Class")]
    public List<CategoryApplicable_PlatformsOperating_SystemsOperating_System_Class> Operating_System_Class
    {
        get
        {
            return _operating_System_Class;
        }
        set
        {
            _operating_System_Class = value;
        }
    }
}
}
#pragma warning restore
