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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType=true, Namespace="http://capec.mitre.org/capec-2")]
public partial class Target_Attack_Surface_DescriptionTypeTarget_Functional_ServiceProtocolRelated_Protocol
{
    #region Private fields
    private List<Target_Attack_Surface_DescriptionTypeTarget_Functional_ServiceProtocolRelated_ProtocolRelationship_Type> _relationship_Type;
    private string _name;
    private string _rFC;
    #endregion
    
    /// <summary>
    /// Target_Attack_Surface_DescriptionTypeTarget_Functional_ServiceProtocolRelated_Protocol class constructor
    /// </summary>
    public Target_Attack_Surface_DescriptionTypeTarget_Functional_ServiceProtocolRelated_Protocol()
    {
        _relationship_Type = new List<Target_Attack_Surface_DescriptionTypeTarget_Functional_ServiceProtocolRelated_ProtocolRelationship_Type>();
    }
    
    [XmlElement("Relationship_Type")]
    public List<Target_Attack_Surface_DescriptionTypeTarget_Functional_ServiceProtocolRelated_ProtocolRelationship_Type> Relationship_Type
    {
        get => _relationship_Type;
        set => _relationship_Type = value;
    }
    
    [XmlAttribute]
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    
    [XmlAttribute]
    public string RFC
    {
        get => _rFC;
        set => _rFC = value;
    }
}
}
#pragma warning restore
