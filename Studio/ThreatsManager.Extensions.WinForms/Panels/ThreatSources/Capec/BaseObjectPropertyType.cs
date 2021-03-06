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
/// The BaseObjectPropertyType is a type representing a common typing foundation for the specification of a single Object Property.
/// </summary>
/// <summary>
/// Properties that use this type can express multiple values by providing them using a delimiter-separated list. The default delimiter is '##comma##' (no quotes) but can be overridden through use of the delimiter field. Note that whitespace is preserved and so, when specifying a list of values, do not include a space following the delimiter in a list unless the first character of the next list item should, in fact, be a space.
/// </summary>
[XmlInclude(typeof(RegionalRegistryType))]
[XmlInclude(typeof(CipherType))]
[XmlInclude(typeof(EndiannessType))]
[XmlInclude(typeof(Layer4ProtocolType))]
[XmlInclude(typeof(SIDType))]
[XmlInclude(typeof(Base64BinaryObjectPropertyType))]
[XmlInclude(typeof(TimeObjectPropertyRestrictionType))]
[XmlInclude(typeof(TimeObjectPropertyType))]
[XmlInclude(typeof(DurationObjectPropertyType))]
[XmlInclude(typeof(AnyURIObjectPropertyType))]
[XmlInclude(typeof(NonNegativeIntegerObjectPropertyType))]
[XmlInclude(typeof(LongObjectPropertyType))]
[XmlInclude(typeof(HexBinaryObjectPropertyType))]
[XmlInclude(typeof(SimpleHashValueType))]
[XmlInclude(typeof(PositiveIntegerObjectPropertyType))]
[XmlInclude(typeof(UnsignedIntegerObjectPropertyType))]
[XmlInclude(typeof(UnsignedLongObjectPropertyType))]
[XmlInclude(typeof(DoubleObjectPropertyType))]
[XmlInclude(typeof(FloatObjectPropertyType))]
[XmlInclude(typeof(DateTimeObjectPropertyRestrictionType))]
[XmlInclude(typeof(DateTimeObjectPropertyType))]
[XmlInclude(typeof(DateObjectPropertyRestrictionType))]
[XmlInclude(typeof(DateObjectPropertyType))]
[XmlInclude(typeof(NameObjectPropertyType))]
[XmlInclude(typeof(StringObjectPropertyType))]
[XmlInclude(typeof(PlatformIdentifierType))]
[XmlInclude(typeof(DataSizeType))]
[XmlInclude(typeof(FuzzyHashValueType))]
[XmlInclude(typeof(IntegerObjectPropertyType))]
[XmlInclude(typeof(PropertyType))]
[XmlInclude(typeof(CompensationModelType))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace="http://cybox.mitre.org/common-2")]
public abstract partial class BaseObjectPropertyType
{
    #region Private fields
    private System.Xml.XmlQualifiedName _id;
    private System.Xml.XmlQualifiedName _idref;
    private DatatypeEnum _datatype;
    private bool _appears_random;
    private bool _is_obfuscated;
    private string _obfuscation_algorithm_ref;
    private bool _is_defanged;
    private string _defanging_algorithm_ref;
    private string _refanging_transform_type;
    private string _refanging_transform;
    private string _observed_encoding;
    private ConditionTypeEnum _condition;
    private bool _is_case_sensitive;
    private ConditionApplicationEnum _apply_condition;
    private string _delimiter;
    private byte[] _bit_mask;
    private PatternTypeEnum _pattern_type;
    private string _regex_syntax;
    private bool _has_changed;
    private bool _trend;
    private string _value;
    #endregion
    
    /// <summary>
    /// BaseObjectPropertyType class constructor
    /// </summary>
    public BaseObjectPropertyType()
    {
        _datatype = DatatypeEnum.@string;
        _is_case_sensitive = true;
        _apply_condition = ConditionApplicationEnum.ANY;
        _delimiter = "##comma##";
    }
    
    [XmlAttribute]
    public System.Xml.XmlQualifiedName id
    {
        get => _id;
        set => _id = value;
    }
    
    [XmlAttribute]
    public System.Xml.XmlQualifiedName idref
    {
        get => _idref;
        set => _idref = value;
    }
    
    [XmlAttribute]
    [DefaultValue(DatatypeEnum.@string)]
    public DatatypeEnum datatype
    {
        get => _datatype;
        set => _datatype = value;
    }
    
    [XmlAttribute]
    public bool appears_random
    {
        get => _appears_random;
        set => _appears_random = value;
    }
    
    [XmlAttribute]
    public bool is_obfuscated
    {
        get => _is_obfuscated;
        set => _is_obfuscated = value;
    }
    
    [XmlAttribute(DataType="anyURI")]
    public string obfuscation_algorithm_ref
    {
        get => _obfuscation_algorithm_ref;
        set => _obfuscation_algorithm_ref = value;
    }
    
    [XmlAttribute]
    public bool is_defanged
    {
        get => _is_defanged;
        set => _is_defanged = value;
    }
    
    [XmlAttribute(DataType="anyURI")]
    public string defanging_algorithm_ref
    {
        get => _defanging_algorithm_ref;
        set => _defanging_algorithm_ref = value;
    }
    
    [XmlAttribute]
    public string refanging_transform_type
    {
        get => _refanging_transform_type;
        set => _refanging_transform_type = value;
    }
    
    [XmlAttribute]
    public string refanging_transform
    {
        get => _refanging_transform;
        set => _refanging_transform = value;
    }
    
    [XmlAttribute]
    public string observed_encoding
    {
        get => _observed_encoding;
        set => _observed_encoding = value;
    }
    
    [XmlAttribute]
    public ConditionTypeEnum condition
    {
        get => _condition;
        set => _condition = value;
    }
    
    [XmlAttribute]
    [DefaultValue(true)]
    public bool is_case_sensitive
    {
        get => _is_case_sensitive;
        set => _is_case_sensitive = value;
    }
    
    [XmlAttribute]
    [DefaultValue(ConditionApplicationEnum.ANY)]
    public ConditionApplicationEnum apply_condition
    {
        get => _apply_condition;
        set => _apply_condition = value;
    }
    
    [XmlAttribute]
    [DefaultValue("##comma##")]
    public string delimiter
    {
        get => _delimiter;
        set => _delimiter = value;
    }
    
    [XmlAttribute(DataType="hexBinary")]
    public byte[] bit_mask
    {
        get => _bit_mask;
        set => _bit_mask = value;
    }
    
    [XmlAttribute]
    public PatternTypeEnum pattern_type
    {
        get => _pattern_type;
        set => _pattern_type = value;
    }
    
    [XmlAttribute]
    public string regex_syntax
    {
        get => _regex_syntax;
        set => _regex_syntax = value;
    }
    
    [XmlAttribute]
    public bool has_changed
    {
        get => _has_changed;
        set => _has_changed = value;
    }
    
    [XmlAttribute]
    public bool trend
    {
        get => _trend;
        set => _trend = value;
    }
    
    [XmlText]
    public string Value
    {
        get => _value;
        set => _value = value;
    }
}
}
#pragma warning restore
