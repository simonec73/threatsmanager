using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.ThreatSources.Capec;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.ThreatSources
{
    public class ThreatSourceNode
    {
        private readonly List<KeyValuePair<string, string>> _properties = new List<KeyValuePair<string, string>>();

        public ThreatSourceNode([NotNull] ThreatSource threatSource, [NotNull] Attack_PatternType attack)
        {
            Id = attack.ID;
            Name = attack.Name;
            IsRoot = false;
            ThreatSourceType = ThreatSourceType.Capec;

            AnalyzeRelationships(threatSource, attack);

            _properties.Add(new KeyValuePair<string, string>("Name", attack.Name));
            if (attack.Description?.Summary != null)
                _properties.Add(new KeyValuePair<string, string>("Summary", Convert(attack.Description.Summary)));
            var alternate = Convert(attack.Alternate_Terms);
            if (!string.IsNullOrWhiteSpace(alternate))
                _properties.Add(new KeyValuePair<string, string>("Alternate Terms", alternate));
            var attackSurface = Convert(attack.Target_Attack_Surface);
            if (!string.IsNullOrWhiteSpace(attackSurface))
                _properties.Add(new KeyValuePair<string, string>("Target Attack Surface", attackSurface));
            var prerequisites = Convert(attack.Attack_Prerequisites);
            if (!string.IsNullOrWhiteSpace(prerequisites))
                _properties.Add(new KeyValuePair<string, string>("Prerequisites", prerequisites));
            _properties.Add(new KeyValuePair<string, string>("Severity", attack.Typical_Severity.ToString()));
            _properties.Add(new KeyValuePair<string, string>("Likelihood of Exploit", attack.Typical_Likelihood_of_Exploit.Likelihood));
            var methodsOfAttack = Convert(attack.Methods_of_Attack);
            if (!string.IsNullOrWhiteSpace(methodsOfAttack))
                _properties.Add(new KeyValuePair<string, string>("Methods of Attack", methodsOfAttack));
            var examples = Convert(attack.ExamplesInstances);
            if (!string.IsNullOrWhiteSpace(examples))
                _properties.Add(new KeyValuePair<string, string>("Examples", examples));
            var skills = Convert(attack.Attacker_Skills_or_Knowledge_Required);
            if (!string.IsNullOrWhiteSpace(skills))
                _properties.Add(new KeyValuePair<string, string>("Skills or Knowledge Required", skills));
            var resources = Convert(attack.Resources_Required);
            if (!string.IsNullOrWhiteSpace(resources))
                _properties.Add(new KeyValuePair<string, string>("Resources Required", resources));
            var probing = Convert(attack.Probing_Techniques);
            if (!string.IsNullOrWhiteSpace(probing))
                _properties.Add(new KeyValuePair<string, string>("Probing Techniques", probing));
            var warning = Convert(attack.IndicatorsWarnings_of_Attack);
            if (!string.IsNullOrWhiteSpace(warning))
                _properties.Add(new KeyValuePair<string, string>("Warning of Attack", warning));
            var obfuscation = Convert(attack.Obfuscation_Techniques);
            if (!string.IsNullOrWhiteSpace(obfuscation))
                _properties.Add(new KeyValuePair<string, string>("Obfuscation Techniques", obfuscation));
            var mitigations = Convert(attack.Solutions_and_Mitigations);
            if (!string.IsNullOrWhiteSpace(mitigations))
                _properties.Add(new KeyValuePair<string, string>("Solutions and Mitigations", mitigations));
            var injectionVector = Convert(attack.Injection_Vector);
            if (!string.IsNullOrWhiteSpace(injectionVector))
                _properties.Add(new KeyValuePair<string, string>("Injection Vector", injectionVector));
            var payload = Convert(attack.Payload);
            if (!string.IsNullOrWhiteSpace(payload))
                _properties.Add(new KeyValuePair<string, string>("Payload", payload));
            var activationZone = Convert(attack.Activation_Zone);
            if (!string.IsNullOrWhiteSpace(activationZone))
                _properties.Add(new KeyValuePair<string, string>("Activation Zone", activationZone));
            var activationImpact = Convert(attack.Payload_Activation_Impact);
            if (!string.IsNullOrWhiteSpace(activationImpact))
                _properties.Add(new KeyValuePair<string, string>("Payload Activation Impact", activationImpact));
            var weaknesses = Convert(attack.Related_Weaknesses);
            if (!string.IsNullOrWhiteSpace(weaknesses))
                _properties.Add(new KeyValuePair<string, string>("Related Weaknesses", weaknesses));
            var vulnerabilities = Convert(attack.Related_Vulnerabilities);
            if (!string.IsNullOrWhiteSpace(vulnerabilities))
                _properties.Add(new KeyValuePair<string, string>("Related Vulnerabilities", vulnerabilities));
            var requirements = Convert(attack.Relevant_Security_Requirements);
            if (!string.IsNullOrWhiteSpace(requirements))
                _properties.Add(new KeyValuePair<string, string>("Security Requirements", requirements));
            var principles = Convert(attack.Related_Security_Principles);
            if (!string.IsNullOrWhiteSpace(principles))
                _properties.Add(new KeyValuePair<string, string>("Security Principles", principles));
            var guidelines = Convert(attack.Related_Guidelines);
            if (!string.IsNullOrWhiteSpace(guidelines))
                _properties.Add(new KeyValuePair<string, string>("Related Guidelines", guidelines));
            var ciaImpact = Convert(attack.CIA_Impact);
            if (!string.IsNullOrWhiteSpace(ciaImpact))
                _properties.Add(new KeyValuePair<string, string>("CIA Impact", ciaImpact));
            var context = Convert(attack.Technical_Context);
            if (!string.IsNullOrWhiteSpace(context))
                _properties.Add(new KeyValuePair<string, string>("Technical Context", context));
            var references = Convert(attack.References);
            if (!string.IsNullOrWhiteSpace(references))
                _properties.Add(new KeyValuePair<string, string>("References", references));
            var notes = Convert(attack.Other_Notes);
            if (!string.IsNullOrWhiteSpace(notes))
                _properties.Add(new KeyValuePair<string, string>("Other Notes", notes));
        }

        public ThreatSourceNode([NotNull] ThreatSource threatSource, [NotNull] Category category)
        {
            Id = category.ID;
            Name = category.Name;
            IsRoot = true;
            ThreatSourceType = ThreatSourceType.Capec;

            AnalyzeRelationships(threatSource, category);

            _properties.Add(new KeyValuePair<string, string>("Name", category.Name));

            if (category.Description?.Description_Summary != null)
                _properties.Add(new KeyValuePair<string, string>("Summary", category.Description.Description_Summary));

            var prerequisites = Convert(category.Attack_Prerequisites);
            if (!string.IsNullOrWhiteSpace(prerequisites))
                _properties.Add(new KeyValuePair<string, string>("Prerequisites", prerequisites));
            var resources = Convert(category.Resources_Required);
            if (!string.IsNullOrWhiteSpace(resources))
                _properties.Add(new KeyValuePair<string, string>("Resources Required", resources));
        }

#if CWE
        public ThreatSourceNode([NotNull] ThreatSource threatSource, [NotNull] Cwe.Category category)
        {
            Id = category.ID;
            Name = category.Name;
            IsRoot = true;
            ThreatSourceType = ThreatSourceType.Cwe;

            //AnalyzeRelationships(threatSource, category);

            //_properties.Add(new KeyValuePair<string, string>("Name", category.Name));

            //if (category.Description?.Description_Summary != null)
            //    _properties.Add(new KeyValuePair<string, string>("Summary", category.Description.Description_Summary));

            //var prerequisites = Convert(category.Attack_Prerequisites);
            //if (!string.IsNullOrWhiteSpace(prerequisites))
            //    _properties.Add(new KeyValuePair<string, string>("Prerequisites", prerequisites));
        }
#endif

        public string Id { get; private set; } 
        public string Name { get; private set; }
        public bool IsRoot { get; private set; }
        public ThreatSourceType ThreatSourceType { get; private set; }
        
        public IEnumerable<KeyValuePair<string, string>> Properties => _properties;

        public bool ApplyFilter(string filter)
        {
            return true;
        }

        #region Private member functions.
        private void AnalyzeRelationships([NotNull] ThreatSource threatSource, [NotNull] Attack_PatternType attack)
        {
            foreach (var relatedAttack in attack.Related_Attack_Patterns)
            {
                if (relatedAttack.Relationship_Target_Form == RelationshipTypeRelationship_Target_Form.AttackPattern &&
                    relatedAttack.Relationship_Nature != null && relatedAttack.Relationship_Nature.Count > 0)
                {
                    switch (relatedAttack.Relationship_Nature[0])
                    {
                        case RelationshipTypeRelationship_Nature.ChildOf:
                            foreach (var view in relatedAttack.Relationship_Views)
                            {
                                threatSource.AddParentChild(view.Value, relatedAttack.Relationship_Target_ID, Id);
                            }
                            break;
                        case RelationshipTypeRelationship_Nature.ParentOf:
                            foreach (var view in relatedAttack.Relationship_Views)
                            {
                                threatSource.AddParentChild(view.Value, Id, relatedAttack.Relationship_Target_ID);
                            }
                            break;
                    }

                }
            }
        }

        private void AnalyzeRelationships([NotNull] ThreatSource threatSource, [NotNull] Category category)
        {
            foreach (var relationthip in category.Relationships)
            {
                if (relationthip.Relationship_Target_Form == RelationshipTypeRelationship_Target_Form.AttackPattern &&
                    relationthip.Relationship_Nature != null && relationthip.Relationship_Nature.Count > 0)
                {
                    switch (relationthip.Relationship_Nature[0])
                    {
                        case RelationshipTypeRelationship_Nature.HasMember:
                            foreach (var view in relationthip.Relationship_Views)
                            {
                                threatSource.AddParentChild(view.Value, Id, relationthip.Relationship_Target_ID);
                            }
                            break;
                    }
                }

            }
        }

        private string Convert(List<Reference_Type> list)
        {
            string result = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in list)
                {
                    builder.AppendLine($"{item.Reference_Author.ConcatenateString()}, {item.Reference_Title}, {item.Reference_Publisher}, {item.Reference_Edition} {item.Reference_PubDate} ({item.Reference_Link})");
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert(List<Alternate_TermsAlternate_Term> list)
        {
            string result = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in list)
                {
                    builder.AppendLine(item.Term);
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert(List<Attack_PatternTypeIndicatorWarning_of_Attack> list)
        {
            string result = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in list)
                {
                    builder.AppendLine(Convert(item.Description));
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert(List<Attack_PatternTypeProbing_Technique> list)
        {
            string result = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in list)
                {
                    builder.AppendLine(Convert(item.Description));
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert(List<Attack_PatternTypeAttacker_Skill_or_Knowledge_Required> list)
        {
            string result = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in list)
                {
                    builder.AppendLine(Convert(item.Skill_or_Knowledge_Type));
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert(List<Attack_PatternTypeExampleInstance> list)
        {
            string result = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in list)
                {
                    builder.AppendLine(Convert(item.ExampleInstance_Description));
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert(List<Attack_PatternTypeMethod_of_Attack> list)
        {
            string result = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in list)
                {
                    builder.AppendLine(item.GetXmlEnumLabel());
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert(List<Attack_PatternTypeObfuscation_Technique> list)
        {
            string result = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in list)
                {
                    if (item.Description != null)
                        builder.AppendLine(Convert(item.Description));
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert(List<Structured_Text_Type> list)
        {
            string result = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in list)
                {
                    builder.AppendLine(Convert(item));
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert(List<Attack_PatternTypeRelated_Weakness> list)
        {
            string result = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in list)
                {
                    builder.AppendLine($"CWE{item.CWE_ID}");
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert(List<Attack_PatternTypeRelated_Vulnerability> list)
        {
            string result = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in list)
                {
                    builder.AppendLine(item.Vulnerability_ID);
                    builder.AppendLine(Convert(item.Vulnerability_Description));
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert([NotNull] Structured_Text_Type item)
        {
#pragma warning disable S3247 // Duplicate casts should not be made
            string result = null;

            if (item?.Items != null && item.Items.Length > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var content in item.Items)
                {
                    if (content is string)
                        builder.AppendLine((string)content);
                    else if (content is Block)
                    {
                        builder.AppendLine(Convert((Block)content));
                    }
                }
                result = builder.ToString();
#pragma warning restore S3247 // Duplicate casts should not be made
            }


            return result;
        }

        private string Convert([NotNull] Block block)
        {
            string result = null;

            if (block.Items.Length > 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var current in block.Items)
                {
                    string currentString = current as string;
                    if (!string.IsNullOrWhiteSpace(currentString))
                    {
                        builder.AppendLine(currentString);
                    }

                    Block currentBlock = current as Block;
                    if (currentBlock != null)
                    {
                        builder.AppendLine(Convert(currentBlock));
                    }

                    if (current is Language_Type)
                    {
                        builder.AppendLine(((Language_Type)current).ToString());
                    }
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert([NotNull] Target_Attack_SurfaceType item)
        {
            string result = null;

            var description = item.Item as Target_Attack_Surface_DescriptionType;
            if (description != null)
            {
                StringBuilder builder = new StringBuilder();

                if (description.Targeted_OSI_Layers?.Any() ?? false)
                {
                    foreach (var layer in description.Targeted_OSI_Layers)
                    {
                        builder.AppendLine($"OSI Layer = {layer.GetXmlEnumLabel()}");
                    }
                }

                if (description.Target_Attack_Surface_Localities?.Any() ?? false)
                {
                    foreach (var localty in description.Target_Attack_Surface_Localities)
                    {
                        builder.AppendLine($"Localty = {localty.GetXmlEnumLabel()}");
                    }
                }

                if (description.Target_Attack_Surface_Types?.Any() ?? false)
                {
                    foreach (var type in description.Target_Attack_Surface_Types)
                    {
                        builder.AppendLine($"Surface Type = {type.GetXmlEnumLabel()}");
                    }
                }

                if (description.Target_Functional_Services?.Any() ?? false)
                {
                    foreach (var service in description.Target_Functional_Services)
                    {
                        builder.AppendLine($"Functional Service = {service.Name}");
                    }
                }

                result = builder.ToString();
            }

            return result;
        }

        private string Convert([NotNull] Attack_PatternTypePayload_Activation_Impact item)
        {
            string result = null;

            if (item.Description != null)
            {
                result = Convert(item.Description);
            }

            return result;
        }

        private string Convert([NotNull] Attack_PatternTypeCIA_Impact item)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Confidentiality: {item.Confidentiality_Impact.ToString()}");
            builder.AppendLine($"Integrity: {item.Integrity_Impact.ToString()}");
            builder.AppendLine($"Availability: {item.Availability_Impact.ToString()}");
            return builder.ToString();
        }

        private string Convert([NotNull] Attack_PatternTypeTechnical_Context item)
        {
            StringBuilder builder = new StringBuilder();

            if (item.Architectural_Paradigms?.Any() ?? false)
            {
                foreach (var paradigm in item.Architectural_Paradigms)
                {
                    builder.AppendLine($"Architectural Paradigm: {paradigm.GetXmlEnumLabel()}");
                }
            }

            if (item.Frameworks?.Any() ?? false)
            {
                foreach (var framework in item.Frameworks)
                {
                    builder.AppendLine($"Framework: {framework.GetXmlEnumLabel()}");
                }
            }

            if (item.Platforms?.Any() ?? false)
            {
                foreach (var platform in item.Platforms)
                {
                    builder.AppendLine($"Platform: {platform.GetXmlEnumLabel()}");
                }
            }

            if (item.Languages?.Any() ?? false)
            {
                foreach (var language in item.Languages)
                {
                    builder.AppendLine($"Language: {language.GetXmlEnumLabel()}");
                }
            }

            return builder.ToString();
        }
        #endregion
    }
}