// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 5.2.97.0. www.xsd2code.com
//  </auto-generated>
// ------------------------------------------------------------------------------
#pragma warning disable
namespace ThreatsManager.Mitre.Cwe
{
using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// The NoteTypeEnumeration simple type defines the different types of notes that can be associated with a weakness. An "Applicable Platform" note provides additional information about the list of applicable platforms for a given weakness. A "Maintenance" note contains significant maintenance tasks within this entry that still need to be addressed, such as clarifying the concepts involved or improving relationships. A "Relationship" note provides clarifying details regarding the relationships between entities. A "Research Gap" note identifies potential opportunities for the vulnerability research community to conduct further exploration of issues related to this weakness. It is intended to highlight parts of CWE that have not received sufficient attention from researchers. A "Terminology" note contains a discussion of terminology issues related to this weakness, or clarifications when there is no established terminology, or if there are multiple uses of the same key term. It is different from the Alternate_Terms element, which is focused on specific terms that are commonly used. A "Theoretical" note describes the weakness using vulnerability theory concepts. It should be provided as needed, especially in cases where the application of vulnerability theory is not necessarily obvious for the weakness.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
[Serializable]
[XmlTypeAttribute(Namespace="http://cwe.mitre.org/cwe-6")]
public enum NoteTypeEnumeration
{
    [XmlEnumAttribute("Applicable Platform")]
    ApplicablePlatform,
    Maintenance,
    Relationship,
    [XmlEnumAttribute("Research Gap")]
    ResearchGap,
    Terminology,
    Theoretical,
    Other,
}
}
#pragma warning restore