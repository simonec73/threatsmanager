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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
public partial class Reference_Type
{
    #region Private fields
    private List<string> _reference_Author;
    private string _reference_Title;
    private string _reference_Section;
    private string _reference_Edition;
    private string _reference_Publication;
    private string _reference_Publisher;
    private DateTime _reference_Date;
    private string _reference_PubDate;
    private string _reference_Link;
    private string _reference_ID;
    private string _local_Reference_ID;
    #endregion
    
    /// <summary>
    /// Reference_Type class constructor
    /// </summary>
    public Reference_Type()
    {
        _reference_Author = new List<string>();
    }
    
    /// <summary>
    /// This element identifies an individual author of the material
    /// being referenced. It is not required, but may be repeated sequentially in
    /// order to identify multiple authors for a single piece of material.
    /// </summary>
    [XmlElement("Reference_Author")]
    public List<string> Reference_Author
    {
        get
        {
            return _reference_Author;
        }
        set
        {
            _reference_Author = value;
        }
    }
    
    /// <summary>
    /// This element identifies the title of the material being
    /// referenced. It is not required if the material does not have a title.
    /// </summary>
    public string Reference_Title
    {
        get
        {
            return _reference_Title;
        }
        set
        {
            _reference_Title = value;
        }
    }
    
    /// <summary>
    /// This element is intended to provide a means of identifying
    /// the exact location of the material inside of the publication source, such as
    /// the relevant pages of a research paper, the appropriate chapters from a
    /// book, etc. This is useful for both book references and internet references.
    /// </summary>
    public string Reference_Section
    {
        get
        {
            return _reference_Section;
        }
        set
        {
            _reference_Section = value;
        }
    }
    
    /// <summary>
    /// This element identifies the edition of the material being
    /// referenced in the event that multiple editions of the material exist. This
    /// will usually only be useful for book references.
    /// </summary>
    public string Reference_Edition
    {
        get
        {
            return _reference_Edition;
        }
        set
        {
            _reference_Edition = value;
        }
    }
    
    /// <summary>
    /// This element identifies the publication source of the
    /// reference material, if one exists.
    /// </summary>
    public string Reference_Publication
    {
        get
        {
            return _reference_Publication;
        }
        set
        {
            _reference_Publication = value;
        }
    }
    
    /// <summary>
    /// This element identifies the publisher of the reference
    /// material, if one exists.
    /// </summary>
    public string Reference_Publisher
    {
        get
        {
            return _reference_Publisher;
        }
        set
        {
            _reference_Publisher = value;
        }
    }
    
    /// <summary>
    /// This element identifies the date when the reference was
    /// included in the entry. This provides the reader with a time line for when
    /// the material in the reference, usually the link, was valid. The date should
    /// be of the format YYYY-MM-DD.
    /// </summary>
    [XmlElement(DataType="date")]
    public DateTime Reference_Date
    {
        get
        {
            return _reference_Date;
        }
        set
        {
            _reference_Date = value;
        }
    }
    
    /// <summary>
    /// This field describes the date when the reference was
    /// published YYYY.
    /// </summary>
    public string Reference_PubDate
    {
        get
        {
            return _reference_PubDate;
        }
        set
        {
            _reference_PubDate = value;
        }
    }
    
    /// <summary>
    /// This element should hold the URL for the material being
    /// referenced, if one exists. This should always be used for web references,
    /// and may optionally be used for book and other publication references.
    /// </summary>
    public string Reference_Link
    {
        get
        {
            return _reference_Link;
        }
        set
        {
            _reference_Link = value;
        }
    }
    
    /// <summary>
    /// The Reference_ID is an optional value for the related Reference
    /// entry identifier as a string. Only one Reference_ID element can exist for each
    /// Reference element (ex: REF-1). However, References across CWE with the same ID
    /// should only vary in small details. Text citing this reference should use the
    /// local reference ID, as this ID is only for reference library related consistency
    /// checking and maintenance.
    /// </summary>
    [XmlAttribute]
    public string Reference_ID
    {
        get
        {
            return _reference_ID;
        }
        set
        {
            _reference_ID = value;
        }
    }
    
    /// <summary>
    /// The Local_Reference_ID is an optional value for the related Local
    /// Reference entry identifier as a string. Only one Local_Reference_ID element can
    /// exist for each Reference element (ex: R.78.1). Text citing this reference should
    /// use the format [R.78.1].
    /// </summary>
    [XmlAttribute]
    public string Local_Reference_ID
    {
        get
        {
            return _local_Reference_ID;
        }
        set
        {
            _local_Reference_ID = value;
        }
    }
}
}
#pragma warning restore
