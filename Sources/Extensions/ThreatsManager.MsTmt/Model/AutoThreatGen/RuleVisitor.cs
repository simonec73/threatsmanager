using System;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoThreatGeneration.Engine;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.MsTmt.Properties;
using ThreatsManager.MsTmt.Schemas;

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
            if ((left?.Length ?? 0) == 2)
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

                result = GetComparisonRuleNode(left[1], right, scope);

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
                result = new EntityTemplateRuleNode(entityTemplate)
                {
                    Scope = scope
                };
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



            return result;
        }

        private IEntityTemplate GetEntityTemplate(string id)
        {
            var schemaManager = new ObjectPropertySchemaManager(_target);
            return _target.EntityTemplates?.FirstOrDefault(x =>
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

            var schemas = _source.GetElementTypesForProperty(propertyKey)?.ToArray();

            if (schemas?.Any() ?? false)
            {
                var schema = schemas.First();
                var propertyName = _source.GetElementPropertyName(schema, propertyKey);
                bool isFlow = false;
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    propertyName = _source.GetFlowPropertyName(schema, propertyKey);
                    schema = Resources.TmtFlowPropertySchema;
                    isFlow = !string.IsNullOrWhiteSpace(propertyName);
                }

                if (isFlow || schemas.Length == 1)
                {
                    result = new ComparisonRuleNode(propertyName,
                        Resources.DefaultNamespace, schema, ComparisonOperator.Exact, value)
                    {
                        Scope = scope
                    };
                }
                else if (schemas.Length > 1)
                {
                    var or = new OrRuleNode()
                    {
                        Name = "OR"
                    };

                    foreach (var s in schemas)
                    {
                        propertyName = _source.GetElementPropertyName(s, propertyKey);
                        if (propertyName != null)
                        {
                            or.Children.Add(new ComparisonRuleNode(propertyName,
                                Resources.DefaultNamespace, s, ComparisonOperator.Exact, value)
                            {
                                Scope = scope
                            });
                        }
                    }

                    result = or;
                }
            }

            return result;
        }
    }
}