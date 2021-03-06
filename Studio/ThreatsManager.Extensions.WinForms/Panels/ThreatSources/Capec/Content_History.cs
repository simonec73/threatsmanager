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
[XmlRoot(Namespace="http://capec.mitre.org/capec-2", IsNullable=false)]
public partial class Content_History
{
    #region Private fields
    private List<Content_HistorySubmission> _submissions;
    private List<Content_HistoryContribution> _contributions;
    private List<Content_HistoryModification> _modifications;
    private List<Content_HistoryPrevious_Entry_Name> _previous_Entry_Names;
    #endregion
    
    /// <summary>
    /// Content_History class constructor
    /// </summary>
    public Content_History()
    {
        _previous_Entry_Names = new List<Content_HistoryPrevious_Entry_Name>();
        _modifications = new List<Content_HistoryModification>();
        _contributions = new List<Content_HistoryContribution>();
        _submissions = new List<Content_HistorySubmission>();
    }
    
    [XmlArrayItem("Submission", IsNullable=false)]
    public List<Content_HistorySubmission> Submissions
    {
        get => _submissions;
        set => _submissions = value;
    }
    
    [XmlArrayItem("Contribution", IsNullable=false)]
    public List<Content_HistoryContribution> Contributions
    {
        get => _contributions;
        set => _contributions = value;
    }
    
    [XmlArrayItem("Modification", IsNullable=false)]
    public List<Content_HistoryModification> Modifications
    {
        get => _modifications;
        set => _modifications = value;
    }
    
    [XmlArrayItem("Previous_Entry_Name", IsNullable=false)]
    public List<Content_HistoryPrevious_Entry_Name> Previous_Entry_Names
    {
        get => _previous_Entry_Names;
        set => _previous_Entry_Names = value;
    }
}
}
#pragma warning restore
