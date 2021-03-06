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
/// The DateObjectPropertyType is a type (extended from BaseObjectPropertyType) representing the specification of a single Object property whose core value is of type Date. This type will be assigned to any property of a CybOX object that should contain content of type Date and enables the use of relevant metadata for the property. In order to avoid ambiguity, it is strongly suggested that any date representation in this field include a timezone if it is known. As with the rest of the field, this should be formatted per the xs:date specification.
/// </summary>
/// <summary>
/// Properties that use this type can express multiple values by providing them using a delimiter-separated list. The default delimiter is '##comma##' (no quotes) but can be overridden through use of the delimiter field. Note that whitespace is preserved and so, when specifying a list of values, do not include a space following the delimiter in a list unless the first character of the next list item should, in fact, be a space.
/// </summary>
/// <summary>
/// For fields of this type using CybOX patterning, it is strongly suggested that the condition (pattern type) is limited to one of Equals, DoesNotEqual, GreaterThan, LessThan, GreaterThanOrEqual, LessThanOrEqual, ExclusiveBetween, or InclusiveBetween. The use of other conditions may lead to ambiguity or unexpected results. When evaluating data against a pattern, the evaluator should take into account the precision of the field (as given by the precision attribute) and any timezone information that is available to perform a data-aware comparison. The usage of simple string comparisons is discouraged due to ambiguities in how precision and timezone information is processed.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace="http://cybox.mitre.org/common-2")]
public partial class DateObjectPropertyType : DateObjectPropertyRestrictionType
{
    #region Private fields
    private DatePrecisionEnum _precision;
    #endregion
    
    /// <summary>
    /// DateObjectPropertyType class constructor
    /// </summary>
    public DateObjectPropertyType()
    {
        _precision = DatePrecisionEnum.day;
    }
    
    /// <summary>
    /// The precision of the associated time. If omitted, the default is "day", meaning the full field value. Digits in the date that are required by the xs:date datatype but are beyond the specified precision should be zeroed out.
    /// </summary>
    /// <summary>
    /// When used in conjunction with CybOX patterning, the pattern should only be evaluated against the target up to the given precision.
    /// </summary>
    [XmlAttribute]
    [DefaultValue(DatePrecisionEnum.day)]
    public DatePrecisionEnum precision
    {
        get => _precision;
        set => _precision = value;
    }
}
}
#pragma warning restore
