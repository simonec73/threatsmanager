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
[XmlType(AnonymousType=true)]
public partial class Structured_Text_TypeImage
{
    #region Private fields
    private List<string> _image_Location;
    private List<string> _image_Title;
    #endregion
    
    /// <summary>
    /// Structured_Text_TypeImage class constructor
    /// </summary>
    public Structured_Text_TypeImage()
    {
        _image_Title = new List<string>();
        _image_Location = new List<string>();
    }
    
    [XmlElement("Image_Location")]
    public List<string> Image_Location
    {
        get
        {
            return _image_Location;
        }
        set
        {
            _image_Location = value;
        }
    }
    
    [XmlElement("Image_Title")]
    public List<string> Image_Title
    {
        get
        {
            return _image_Title;
        }
        set
        {
            _image_Title = value;
        }
    }
}
}
#pragma warning restore
