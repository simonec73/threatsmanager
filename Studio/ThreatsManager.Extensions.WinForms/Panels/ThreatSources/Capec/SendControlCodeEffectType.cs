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
/// The SendControlCodeEffectType is intended to characterize the effects of actions upon objects where some control code, or other control-oriented communication signal, is sent to the object. For example, an action may send a control code to change the running state of a process.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace="http://cybox.mitre.org/cybox-2")]
public partial class SendControlCodeEffectType : DefinedEffectType
{
    #region Private fields
    private string _control_Code;
    #endregion
    
    /// <summary>
    /// The Control_Code field specifies the actual control code that was sent to the object.
    /// </summary>
    public string Control_Code
    {
        get => _control_Code;
        set => _control_Code = value;
    }
}
}
#pragma warning restore
