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
/// The ContributorType represents a description of an individual who contributed as a source of cyber observation data.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace="http://cybox.mitre.org/common-2")]
public partial class ContributorType
{
    #region Private fields
    private string _role;
    private string _name;
    private string _email;
    private string _phone;
    private string _organization;
    private DateRangeType _date;
    private string _contribution_Location;
    #endregion
    
    /// <summary>
    /// ContributorType class constructor
    /// </summary>
    public ContributorType()
    {
        _date = new DateRangeType();
    }
    
    /// <summary>
    /// This field describes the role played by this contributor.
    /// </summary>
    public string Role
    {
        get => _role;
        set => _role = value;
    }
    
    /// <summary>
    /// This field contains the name of this contributor.
    /// </summary>
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    
    /// <summary>
    /// This field contains the email of this contributor.
    /// </summary>
    public string Email
    {
        get => _email;
        set => _email = value;
    }
    
    /// <summary>
    /// This field contains a telephone number of this contributor.
    /// </summary>
    public string Phone
    {
        get => _phone;
        set => _phone = value;
    }
    
    /// <summary>
    /// This field contains the organization name of this contributor.
    /// </summary>
    public string Organization
    {
        get => _organization;
        set => _organization = value;
    }
    
    /// <summary>
    /// This field contains a description (bounding) of the timing of this contributor's involvement.
    /// </summary>
    public DateRangeType Date
    {
        get => _date;
        set => _date = value;
    }
    
    /// <summary>
    /// This field contains information describing the location at which the contributory activity occured.
    /// </summary>
    public string Contribution_Location
    {
        get => _contribution_Location;
        set => _contribution_Location = value;
    }
}
}
#pragma warning restore
