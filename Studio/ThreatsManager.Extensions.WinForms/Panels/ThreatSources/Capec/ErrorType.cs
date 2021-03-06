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
namespace ThreatsManager.Extensions.Panels.ThreatSources.Capec
{
    /// <summary>
/// The ErrorType captures a single error generated during the run of the tool.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace="http://cybox.mitre.org/common-2")]
public partial class ErrorType
{
    #region Private fields
    private string _error_Type;
    private string _error_Count;
    private List<string> _error_Instances;
    #endregion
    
    /// <summary>
    /// ErrorType class constructor
    /// </summary>
    public ErrorType()
    {
        _error_Instances = new List<string>();
    }
    
    /// <summary>
    /// This field specifies the type for this tool run error.
    /// </summary>
    public string Error_Type
    {
        get => _error_Type;
        set => _error_Type = value;
    }
    
    /// <summary>
    /// This field specifies the count of instances for this error in the tool run.
    /// </summary>
    [XmlElement(DataType="integer")]
    public string Error_Count
    {
        get => _error_Count;
        set => _error_Count = value;
    }
    
    /// <summary>
    /// This field captures the actual error output for each instance of this type of error.
    /// </summary>
    [XmlArrayItem("Error_Instance", IsNullable=false)]
    public List<string> Error_Instances
    {
        get => _error_Instances;
        set => _error_Instances = value;
    }
}
}
#pragma warning restore
