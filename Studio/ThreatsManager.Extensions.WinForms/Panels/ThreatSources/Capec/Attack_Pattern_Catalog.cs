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
public partial class Attack_Pattern_Catalog
{
    #region Private fields
    private List<View> _views;
    private List<Category> _categories;
    private List<Attack_PatternType> _attack_Patterns;
    private Attack_Pattern_CatalogCompound_Elements _compound_Elements;
    private List<Attack_Pattern_CatalogCommon_Attack_Step> _common_Attack_Steps;
    private List<Attack_Pattern_CatalogCommon_Attack_Surface> _common_Attack_Surfaces;
    private List<Environment> _environments;
    private string _catalog_Name;
    private string _catalog_Version;
    private DateTime _catalog_Date;
    #endregion
    
    /// <summary>
    /// Attack_Pattern_Catalog class constructor
    /// </summary>
    public Attack_Pattern_Catalog()
    {
        _environments = new List<Environment>();
        _common_Attack_Surfaces = new List<Attack_Pattern_CatalogCommon_Attack_Surface>();
        _common_Attack_Steps = new List<Attack_Pattern_CatalogCommon_Attack_Step>();
        _compound_Elements = new Attack_Pattern_CatalogCompound_Elements();
        _attack_Patterns = new List<Attack_PatternType>();
        _categories = new List<Category>();
        _views = new List<View>();
    }
    
    [XmlArrayItem("View", IsNullable=false)]
    public List<View> Views
    {
        get => _views;
        set => _views = value;
    }
    
    [XmlArrayItem("Category", IsNullable=false)]
    public List<Category> Categories
    {
        get => _categories;
        set => _categories = value;
    }
    
    [XmlArrayItem("Attack_Pattern", IsNullable=false)]
    public List<Attack_PatternType> Attack_Patterns
    {
        get => _attack_Patterns;
        set => _attack_Patterns = value;
    }
    
    public Attack_Pattern_CatalogCompound_Elements Compound_Elements
    {
        get => _compound_Elements;
        set => _compound_Elements = value;
    }
    
    [XmlArrayItem("Common_Attack_Step", IsNullable=false)]
    public List<Attack_Pattern_CatalogCommon_Attack_Step> Common_Attack_Steps
    {
        get => _common_Attack_Steps;
        set => _common_Attack_Steps = value;
    }
    
    [XmlArrayItem("Common_Attack_Surface", IsNullable=false)]
    public List<Attack_Pattern_CatalogCommon_Attack_Surface> Common_Attack_Surfaces
    {
        get => _common_Attack_Surfaces;
        set => _common_Attack_Surfaces = value;
    }
    
    [XmlArrayItem("Environment", IsNullable=false)]
    public List<Environment> Environments
    {
        get => _environments;
        set => _environments = value;
    }
    
    [XmlAttribute]
    public string Catalog_Name
    {
        get => _catalog_Name;
        set => _catalog_Name = value;
    }
    
    [XmlAttribute]
    public string Catalog_Version
    {
        get => _catalog_Version;
        set => _catalog_Version = value;
    }
    
    [XmlAttribute(DataType="date")]
    public DateTime Catalog_Date
    {
        get => _catalog_Date;
        set => _catalog_Date = value;
    }
}
}
#pragma warning restore
