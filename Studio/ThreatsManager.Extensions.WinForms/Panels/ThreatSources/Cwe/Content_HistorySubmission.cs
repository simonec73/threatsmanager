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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType=true)]
public partial class Content_HistorySubmission
{
    #region Private fields
    private string _submitter;
    private string _submitter_Organization;
    private DateTime _submission_Date;
    private string _submission_Comment;
    private Content_HistorySubmissionSubmission_Source _submission_Source;
    #endregion
    
    public string Submitter
    {
        get
        {
            return _submitter;
        }
        set
        {
            _submitter = value;
        }
    }
    
    public string Submitter_Organization
    {
        get
        {
            return _submitter_Organization;
        }
        set
        {
            _submitter_Organization = value;
        }
    }
    
    [XmlElement(DataType="date")]
    public DateTime Submission_Date
    {
        get
        {
            return _submission_Date;
        }
        set
        {
            _submission_Date = value;
        }
    }
    
    public string Submission_Comment
    {
        get
        {
            return _submission_Comment;
        }
        set
        {
            _submission_Comment = value;
        }
    }
    
    [XmlAttribute]
    public Content_HistorySubmissionSubmission_Source Submission_Source
    {
        get
        {
            return _submission_Source;
        }
        set
        {
            _submission_Source = value;
        }
    }
}
}
#pragma warning restore
