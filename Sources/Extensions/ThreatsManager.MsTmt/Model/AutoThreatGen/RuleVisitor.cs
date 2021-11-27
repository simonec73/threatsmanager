using System;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.MsTmt.Properties;
using ThreatsManager.MsTmt.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Model.AutoThreatGen
{
    class RuleVisitor : TmtBaseVisitor<SelectionRuleNode>
    {
        private readonly ThreatModel _source;
        private readonly IThreatModel _target;

        public RuleVisitor([NotNull] ThreatModel source, [NotNull] IThreatModel target)
        {
            _source = source;
            _target = target;
        }

        public SelectionRuleNode Rule { get; private set; }

        public override SelectionRuleNode VisitBooleanExpression(TmtParser.BooleanExpressionContext context)
        {
            SelectionRuleNode result;

            NaryRuleNode rule = null;
            switch (context.op?.GetText())
            {
                case "and":
                    rule = new AndRuleNode()
                    {
                        Name = "AND"
                    };
                    break;
                case "or":
                    rule = new OrRuleNode()
                    {
                        Name = "OR"
                    };
                    break;
            }

            if (Rule == null)
                Rule = rule;

            var left = Visit(context.left);
            var right = Visit(context.right);
            if (left != null)
            {
                if (right != null)
                {
                    rule?.Children.Add(left);
                    rule?.Children.Add(right);

                    result = rule;
                }
                else
                {
                    result = left;
                    Rule = left;
                }
            }
            else
            {
                result = right;
                Rule = right;
            }

            return result;
        }

        public override SelectionRuleNode VisitFlowExpression(TmtParser.FlowExpressionContext context)
        {
            SelectionRuleNode result = null;

            var id = context.right?.Text?.Trim('\'');
            if (!string.IsNullOrWhiteSpace(id))
            {
                switch (context.op?.GetText())
                {
                    case "is":
                        result = GetIdComparisonRuleNode(id, Scope.Object);
                        break;
                    case "crosses":
                        var template = GetTrustBoundaryTemplate(id);
                        if (template != null)
                            result = new TrustBoundaryTemplateRuleNode(template);
                        else
                        {
                            if (_source.ElementTypes?.Any(x => x.IsGeneric &&
                                                               (x.ElementType == ElementType.BorderBoundary || x.ElementType == ElementType.LineBoundary) &&
                                                               string.CompareOrdinal(x.TypeId, id) == 0) ?? false)
                                result = new CrossTrustBoundaryRuleNode("Crosses Trust Boundary", true);
                        }
                        break;
                }

                if (Rule == null)
                    Rule = result;
            }

            return result;
        }

        public override SelectionRuleNode VisitNotExpression(TmtParser.NotExpressionContext context)
        {
            SelectionRuleNode result = null;

            var not = new NotRuleNode()
            {
                Name = "NOT"
            };
            if (Rule == null)
                Rule = not;
            
            var expression = Visit(context.expression());
            if (expression != null)
            {
                not.Child = expression;
                result = not;
            }
            else
            {
                Rule = null;
            }

            return result;
        }

        public override SelectionRuleNode VisitParentExpression(TmtParser.ParentExpressionContext context)
        {
            return Visit(context.expression());
        }

        public override SelectionRuleNode VisitSubjectExpression(TmtParser.SubjectExpressionContext context)
        {
            SelectionRuleNode result = null;

            var sourceTarget = context.left?.GetText();
            var id = context.right?.Text?.Trim('\'');

            if (!string.IsNullOrWhiteSpace(id) && string.CompareOrdinal(id, "ROOT") != 0 &&
                Enum.TryParse<Scope>(sourceTarget, true, out var scope))
            {
                result = GetIdComparisonRuleNode(id, scope);
                if (Rule == null)
                    Rule = result;
            }

            return result;
        }

        public override SelectionRuleNode VisitPropertyExpression(TmtParser.PropertyExpressionContext context)
        {
            SelectionRuleNode result = null;

            var left = context.left?.GetText()?.Split('.');
            var right = context.right?.Text?.Trim('\'');
            if ((left?.Length ?? 0) >= 2)
            {
                var scope = Scope.Object;

                // ReSharper disable once PossibleNullReferenceException
                switch (left[0])
                {
                    case "source":
                        scope = Scope.Source;
                        break;
                    case "target":
                        scope = Scope.Target;
                        break;
                }

                string propertyName = left.Length == 2 ? left[1] : left.Skip(1).Concat('.', null);
                result = GetComparisonRuleNode(propertyName, right, scope);

                if (Rule == null)
                    Rule = result;
            }

            return result;
        }

        private SelectionRuleNode Visit(TmtParser.ExpressionContext context)
        {
            SelectionRuleNode result = null;

            if (context is TmtParser.BooleanExpressionContext booleanExpressionContext)
            {
                result = VisitBooleanExpression(booleanExpressionContext);
            }
            else if (context is TmtParser.FlowExpressionContext flowExpressionContext)
            {
                result = VisitFlowExpression(flowExpressionContext);
            }
            else if (context is TmtParser.NotExpressionContext notExpressionContext)
            {
                result = VisitNotExpression(notExpressionContext);
            }
            else if (context is TmtParser.ParentExpressionContext parentExpressionContext)
            {
                result = VisitParentExpression(parentExpressionContext);
            }
            else if (context is TmtParser.SubjectExpressionContext subjectExpressionContext)
            {
                result = VisitSubjectExpression(subjectExpressionContext);
            }
            else if (context is TmtParser.PropertyExpressionContext propertyExpressionContext)
            {
                result = VisitPropertyExpression(propertyExpressionContext);
            }

            return result;
        }

        private SelectionRuleNode GetIdComparisonRuleNode([Required] string id, Scope scope)
        {
            SelectionRuleNode result = null;

            var elementType = _source.ElementTypes?.FirstOrDefault(x => string.CompareOrdinal(x.TypeId, id) == 0);

            var entityTemplate = GetEntityTemplate(id);

            if (elementType?.IsGeneric ?? false)
            {
                switch (elementType.ElementType)
                {
                    case ElementType.StencilRectangle:
                        result = new EnumValueRuleNode("Object Type", null, null,
                            new[] { "External Interactor", "Process", "Data Store" },
                            "External Interactor")
                        {
                            Scope = scope
                        };
                        break;
                    case ElementType.StencilEllipse:
                        result = new EnumValueRuleNode("Object Type", null, null,
                            new[] { "External Interactor", "Process", "Data Store" },
                            "Process")
                        {
                            Scope = scope
                        };
                        break;
                    case ElementType.StencilParallelLines:
                        result = new EnumValueRuleNode("Object Type", null, null,
                            new[] { "External Interactor", "Process", "Data Store" },
                            "Data Store")
                        {
                            Scope = scope
                        };
                        break;
                }
            }
            else if (entityTemplate != null)
            {
                switch (entityTemplate.EntityType)
                {
                    case EntityType.ExternalInteractor:
                        result = new ExternalInteractorTemplateRuleNode(entityTemplate)
                        {
                            Scope = scope
                        };
                        break;
                    case EntityType.Process:
                        result = new ProcessTemplateRuleNode(entityTemplate)
                        {
                            Scope = scope
                        };
                        break;
                    case EntityType.DataStore:
                        result = new DataStoreTemplateRuleNode(entityTemplate)
                        {
                            Scope = scope
                        };
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }
            else
            {
                var flowTemplate = GetFlowTemplate(id);
                if (flowTemplate != null)
                {
                    result = new FlowTemplateRuleNode(flowTemplate);
                }
                else
                {
                    result = new ComparisonRuleNode(ObjectPropertySchemaManager.ThreatModelObjectId,
                        Resources.DefaultNamespace, Resources.TmtObjectPropertySchema,
                        ComparisonOperator.Exact, id)
                    {
                        Scope = scope
                    };
                }
            }



            return result;
        }

        private IEntityTemplate GetEntityTemplate(string id)
        {
            var schemaManager = new ObjectPropertySchemaManager(_target);
            return _target.EntityTemplates?.FirstOrDefault(x =>
                string.CompareOrdinal(id, schemaManager.GetObjectId(x)) == 0);
        }

        private IFlowTemplate GetFlowTemplate(string id)
        {
            var schemaManager = new ObjectPropertySchemaManager(_target);
            return _target.FlowTemplates?.FirstOrDefault(x =>
                string.CompareOrdinal(id, schemaManager.GetObjectId(x)) == 0);
        }

        private ITrustBoundaryTemplate GetTrustBoundaryTemplate(string id)
        {
            var schemaManager = new ObjectPropertySchemaManager(_target);
            return _target.TrustBoundaryTemplates?.FirstOrDefault(x =>
                string.CompareOrdinal(id, schemaManager.GetObjectId(x)) == 0);
        }

        private SelectionRuleNode GetComparisonRuleNode([Required] string propertyKey, string value, Scope scope)
        {
            SelectionRuleNode result = null;

            var elementSchemas = _source.GetElementTypesForProperty(propertyKey)?.ToArray();
            var flowSchemas = _source.GetFlowTypesForProperty(propertyKey)?.ToArray();

            var schema = elementSchemas?.FirstOrDefault();
            var property = schema?.Properties?.FirstOrDefault(x => string.CompareOrdinal(x.Key, propertyKey) == 0);
            string schemaName;
            if (schema != null && schema.IsGeneric)
            {
                switch (schema.ElementType)
                {
                    case ElementType.StencilRectangle:
                        schemaName = Resources.TmtExternalInteractorPropertySchema;
                        break;
                    case ElementType.StencilEllipse:
                        schemaName = Resources.TmtProcessPropertySchema;
                        break;
                    case ElementType.StencilParallelLines:
                        schemaName = Resources.TmtDataStorePropertySchema;
                        break;
                    case ElementType.BorderBoundary:
                        schemaName = Resources.TmtTrustBoundaryPropertySchema;
                        break;
                    case ElementType.Connector:
                        schemaName = Resources.TmtFlowPropertySchema;
                        break;
                    case ElementType.LineBoundary:
                        schemaName = Resources.TmtTrustBoundaryPropertySchema;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                schemaName = schema?.Name;
            }

            if (property == null)
            {
                schema = flowSchemas?.FirstOrDefault();
                property = schema?.Properties?.FirstOrDefault(x => string.CompareOrdinal(x.Key, propertyKey) == 0);
                if (schema?.IsGeneric ?? false)
                    schemaName = Resources.TmtFlowPropertySchema;
                else
                {
                    schemaName = schema?.Name;
                }
            }

            if (schemaName != null && property != null)
            {
                switch (property.Type)
                {
                    case PropertyType.String:
                        result = new ComparisonRuleNode(property.Name,
                            Resources.DefaultNamespace, schemaName, ComparisonOperator.Exact, value)
                        {
                            Scope = scope
                        };
                        break;
                    case PropertyType.Boolean:
                        if (bool.TryParse(value, out var boolValue))
                            result = new BooleanRuleNode(property.Name, Resources.DefaultNamespace, schemaName, boolValue)
                            {
                                Scope = scope
                            };
                        break;
                    case PropertyType.List:
                        result = new EnumValueRuleNode(property.Name, Resources.DefaultNamespace, schemaName,
                            property.Values, value)
                        {
                            Scope = scope
                        };
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        }
    }
}