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
/// This is the enumerated catalog of common attack patterns.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType=true, Namespace="http://capec.mitre.org/capec-2")]
[XmlRoot(Namespace="http://capec.mitre.org/capec-2", IsNullable=false)]
public partial class Category
{
    #region Private fields
    private CategoryDescription _description;
    private List<CategoryRelated_Weakness> _related_Weaknesses;
    private List<Structured_Text_Type> _attack_Prerequisites;
    private List<CategoryMethod_of_Attack> _methods_of_Attack;
    private List<CategoryAttacker_Skill_or_Knowledge_Required> _attacker_Skills_or_Knowledge_Required;
    private Structured_Text_Type _resources_Required;
    private List<Common_ConsequenceType> _attack_MotivationConsequences;
    private List<RelationshipType> _relationships;
    private List<Structured_Text_Type> _relationship_Notes;
    private List<Structured_Text_Type> _maintenance_Notes;
    private List<Structured_Text_Type> _background_Details;
    private List<Structured_Text_Type> _other_Notes;
    private List<Alternate_TermsAlternate_Term> _alternate_Terms;
    private List<Structured_Text_Type> _research_Gaps;
    private List<Reference_Type> _references;
    private Content_History _content_History;
    private string _id;
    private string _name;
    private Status_Type _status;
    #endregion
    
    /// <summary>
    /// Category class constructor
    /// </summary>
    public Category()
    {
        _content_History = new Content_History();
        _references = new List<Reference_Type>();
        _research_Gaps = new List<Structured_Text_Type>();
        _alternate_Terms = new List<Alternate_TermsAlternate_Term>();
        _other_Notes = new List<Structured_Text_Type>();
        _background_Details = new List<Structured_Text_Type>();
        _maintenance_Notes = new List<Structured_Text_Type>();
        _relationship_Notes = new List<Structured_Text_Type>();
        _relationships = new List<RelationshipType>();
        _attack_MotivationConsequences = new List<Common_ConsequenceType>();
        _resources_Required = new Structured_Text_Type();
        _attacker_Skills_or_Knowledge_Required = new List<CategoryAttacker_Skill_or_Knowledge_Required>();
        _methods_of_Attack = new List<CategoryMethod_of_Attack>();
        _attack_Prerequisites = new List<Structured_Text_Type>();
        _related_Weaknesses = new List<CategoryRelated_Weakness>();
        _description = new CategoryDescription();
    }
    
    /// <summary>
    /// This field provides a description of this Category. Its primary subelement is Description_Summary which is intended to serve as a minimalistic description which provides the information necessary to understand the primary focus of this entry. Additionally, it has the subelement Extended_Description which is optional and is used to provide further information pertaining to this attack pattern.
    /// </summary>
    public CategoryDescription Description
    {
        get => _description;
        set => _description = value;
    }
    
    /// <summary>
    /// Which specific weaknesses does this attack target and leverage? Specific weaknesses (underlying issues that may cause vulnerabilities) reference the industry-standard Common Weakness Enumeration (CWE). This list should include not only those weaknesses that are directly targeted by the attack but also those whose presence can directly increase the likelihood of the attack succeeding or the impact if it does succeed.
    /// </summary>
    [XmlArrayItem("Related_Weakness", IsNullable=false)]
    public List<CategoryRelated_Weakness> Related_Weaknesses
    {
        get => _related_Weaknesses;
        set => _related_Weaknesses = value;
    }
    
    /// <summary>
    /// This field describes the conditions that must exist or the functionality and characteristics that the target software must have or behavior it must exhibit for an attack of this type to succeed.
    /// </summary>
    [XmlArrayItem("Attack_Prerequisite", IsNullable=false)]
    public List<Structured_Text_Type> Attack_Prerequisites
    {
        get => _attack_Prerequisites;
        set => _attack_Prerequisites = value;
    }
    
    /// <summary>
    /// This field describes the mechanism of attack used by this pattern. This field can help define the applicable attack surface required for this attack.
    /// </summary>
    [XmlArrayItem("Method_of_Attack", IsNullable=false)]
    public List<CategoryMethod_of_Attack> Methods_of_Attack
    {
        get => _methods_of_Attack;
        set => _methods_of_Attack = value;
    }
    
    /// <summary>
    /// This field describes the level of skills or specific knowledge required by an attacker to execute this type of attack.
    /// </summary>
    [XmlArrayItem("Attacker_Skill_or_Knowledge_Required", IsNullable=false)]
    public List<CategoryAttacker_Skill_or_Knowledge_Required> Attacker_Skills_or_Knowledge_Required
    {
        get => _attacker_Skills_or_Knowledge_Required;
        set => _attacker_Skills_or_Knowledge_Required = value;
    }
    
    /// <summary>
    /// This field describes the resources (CPU cycles, IP addresses, tools, etc.) required by an attacker to effectively execute this type of attack.
    /// </summary>
    public Structured_Text_Type Resources_Required
    {
        get => _resources_Required;
        set => _resources_Required = value;
    }
    
    [XmlArray("Attack_Motivation-Consequences")]
    [XmlArrayItem("Attack_Motivation-Consequence", IsNullable=false)]
    public List<Common_ConsequenceType> Attack_MotivationConsequences
    {
        get => _attack_MotivationConsequences;
        set => _attack_MotivationConsequences = value;
    }
    
    /// <summary>
    /// The Relationships structure contains one or more Relationship elements, each of which identifies an association between this structure, whether it is an Attack Pattern, Category, or Compound_Element and another structure.
    /// </summary>
    [XmlArrayItem("Relationship", IsNullable=false)]
    public List<RelationshipType> Relationships
    {
        get => _relationships;
        set => _relationships = value;
    }
    
    /// <summary>
    /// This structure houses one or more Relationship_Note elements, which each contain details regarding the relationships between CAPEC entries.
    /// </summary>
    [XmlArrayItem("Relationship_Note", IsNullable=false)]
    public List<Structured_Text_Type> Relationship_Notes
    {
        get => _relationship_Notes;
        set => _relationship_Notes = value;
    }
    
    /// <summary>
    /// This element contains one or more Maintenance_Note elements which each contain significant maintenance tasks within this entry that still need to be addressed, such as clarifying the concepts involved or improving relationships. It should be filled out in any entry that is still undergoing significant review by the CAPEC team.
    /// </summary>
    [XmlArrayItem("Maintenance_Note", IsNullable=false)]
    public List<Structured_Text_Type> Maintenance_Notes
    {
        get => _maintenance_Notes;
        set => _maintenance_Notes = value;
    }
    
    /// <summary>
    /// This structure contains one or more Background_Detail elements, each of which holds information regarding the entry or any technologies that are related to it, where the background information is not related to the nature of the entry itself. It should be filled out where appropriate.
    /// </summary>
    [XmlArrayItem("Background_Detail", IsNullable=false)]
    public List<Structured_Text_Type> Background_Details
    {
        get => _background_Details;
        set => _background_Details = value;
    }
    
    /// <summary>
    /// This element contains one or more Note elements, each of which provide any additional notes or comments that cannot be captured using other elements. New elements might be defined in the future to contain this information. It should be filled out where needed.
    /// </summary>
    [XmlArrayItem("Note", IsNullable=false)]
    public List<Structured_Text_Type> Other_Notes
    {
        get => _other_Notes;
        set => _other_Notes = value;
    }
    
    /// <summary>
    /// This element contains one or more Alternate_Term elements, each of which contains other names used to describe this attack pattern.
    /// </summary>
    [XmlArrayItem("Alternate_Term", IsNullable=false)]
    public List<Alternate_TermsAlternate_Term> Alternate_Terms
    {
        get => _alternate_Terms;
        set => _alternate_Terms = value;
    }
    
    /// <summary>
    /// This structure contains one or more Research gap elements, each of which identifies potential opportunities for the vulnerability research community to conduct further exploration of issues related to this attack pattern. It is intended to highlight parts of CAPEC that have not received sufficient attention from researchers. This should be filled out where appropriate for attack patterns and categories.
    /// </summary>
    [XmlArrayItem("Research_Gap", IsNullable=false)]
    public List<Structured_Text_Type> Research_Gaps
    {
        get => _research_Gaps;
        set => _research_Gaps = value;
    }
    
    /// <summary>
    /// The References element contains one or more Reference elements, each of which provide further reading and insight into this attack pattern.
    /// </summary>
    [XmlArrayItem("Reference", IsNullable=false)]
    public List<Reference_Type> References
    {
        get => _references;
        set => _references = value;
    }
    
    /// <summary>
    /// This element is used to keep track of the author of the attack pattern entry and anyone who has made modifications to the content. This provides a means of contacting the authors and modifiers for clarifying ambiguities, merging overlapping contributions, etc. This should be filled out for all entries.
    /// </summary>
    public Content_History Content_History
    {
        get => _content_History;
        set => _content_History = value;
    }
    
    [XmlAttribute(DataType="integer")]
    public string ID
    {
        get => _id;
        set => _id = value;
    }
    
    /// <summary>
    /// This field contains the name of this contributor.
    /// </summary>
    [XmlAttribute]
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    
    [XmlAttribute]
    public Status_Type Status
    {
        get => _status;
        set => _status = value;
    }
}
}
#pragma warning restore
