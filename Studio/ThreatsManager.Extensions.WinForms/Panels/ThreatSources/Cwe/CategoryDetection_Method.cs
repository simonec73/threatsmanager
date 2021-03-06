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
/// The Detection_Method element is intended to
/// provide information on different techniques that can be used to
/// detect a weakness, including their strengths and limitations.
/// This should be filled out for some weakness classes and bases.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType=true)]
public partial class CategoryDetection_Method
{
    #region Private fields
    private CategoryDetection_MethodMethod_Name _method_Name;
    private Structured_Text_Type _method_Description;
    private CategoryDetection_MethodMethod_Effectiveness _method_Effectiveness;
    private Structured_Text_Type _method_Effectiveness_Notes;
    private string _detection_Method_ID;
    #endregion
    
    /// <summary>
    /// CategoryDetection_Method class constructor
    /// </summary>
    public CategoryDetection_Method()
    {
        _method_Effectiveness_Notes = new Structured_Text_Type();
        _method_Description = new Structured_Text_Type();
    }
    
    /// <summary>
    /// The Method_Name element identifies the
    /// particular weakness detection method to be
    /// described. This should be filled out for some
    /// weakness classes and bases.
    /// </summary>
    public CategoryDetection_MethodMethod_Name Method_Name
    {
        get
        {
            return _method_Name;
        }
        set
        {
            _method_Name = value;
        }
    }
    
    /// <summary>
    /// The Method_Description element is
    /// intended to provide some context of how this
    /// Detection_Method can be applied to a specific
    /// weakness. This should be filled out for some
    /// weakness classes and bases.
    /// </summary>
    public Structured_Text_Type Method_Description
    {
        get
        {
            return _method_Description;
        }
        set
        {
            _method_Description = value;
        }
    }
    
    /// <summary>
    /// This element summarizes how effective
    /// the detection method may be in detecting the
    /// associated weakness. This assumes the use of
    /// best-of-breed tools, analysts, and methods. There is
    /// limited consideration for financial costs, labor, or
    /// time.
    /// </summary>
    public CategoryDetection_MethodMethod_Effectiveness Method_Effectiveness
    {
        get
        {
            return _method_Effectiveness;
        }
        set
        {
            _method_Effectiveness = value;
        }
    }
    
    /// <summary>
    /// The Method_Effectiveness_Notes
    /// element is intended to discuss the strengths and
    /// shortcomings of this detection method. This should
    /// be filled out for some weakness classes and bases.
    /// </summary>
    public Structured_Text_Type Method_Effectiveness_Notes
    {
        get
        {
            return _method_Effectiveness_Notes;
        }
        set
        {
            _method_Effectiveness_Notes = value;
        }
    }
    
    /// <summary>
    /// The Detection_Method_ID stores the value
    /// for the related Detection_Method entry identifier as a
    /// string. Only one Detection_Method_ID element can exist
    /// for each Detection_Method element (ex: DM-1). However,
    /// Detection_Methods across CWE with the same ID should
    /// only vary in small details.
    /// </summary>
    [XmlAttribute]
    public string Detection_Method_ID
    {
        get
        {
            return _detection_Method_ID;
        }
        set
        {
            _detection_Method_ID = value;
        }
    }
}
}
#pragma warning restore
