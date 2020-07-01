using System;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoThreatGeneration.Engine;
using ThreatsManager.MsTmt.Properties;
using ThreatsManager.MsTmt.Schemas;

namespace ThreatsManager.MsTmt.Model.AutoThreatGen
{
    class RuleVisitor : TmtBaseVisitor<SelectionRuleNode>
    {
        private readonly ThreatModel _model;

        public RuleVisitor(ThreatModel model)
        {
            _model = model;
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
                        result = GetIdComparisonRuleNode(id, Scope.AnyTrustBoundary);
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
                Rule = result;
            
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

            var children = _model.GetChildren(id)?.ToArray();

            if (children?.Any() ?? false)
            {
                var or = new OrRuleNode()
                {
                    Name = "OR"
                };

                or.Children.Add(new ComparisonRuleNode(ObjectPropertySchemaManager.ThreatModelObjectId,
                    Resources.DefaultNamespace, Resources.TmtObjectPropertySchema, ComparisonOperator.Exact, id)
                {
                    Scope = scope
                });

                foreach (var child in children)
                {
                    or.Children.Add(new ComparisonRuleNode(ObjectPropertySchemaManager.ThreatModelObjectId,
                        Resources.DefaultNamespace, Resources.TmtObjectPropertySchema, ComparisonOperator.Exact, child.TypeId)
                    {
                        Scope = scope
                    });
                }

                result = or;
            }
            else
            {
                result = new ComparisonRuleNode(ObjectPropertySchemaManager.ThreatModelObjectId,
                    Resources.DefaultNamespace, Resources.TmtObjectPropertySchema, ComparisonOperator.Exact, id)
                {
                    Scope = scope
                };
            }


            return result;
        }

        private SelectionRuleNode GetComparisonRuleNode([Required] string propertyKey, string value, Scope scope)
        {
            SelectionRuleNode result = null;

            var schemas = _model.GetElementTypesForProperty(propertyKey)?.ToArray();

            if (schemas?.Any() ?? false)
            {
                if (schemas.Length == 1)
                {
                    var schema = schemas.First();
                    var propertyName = _model.GetPropertyName(schema, propertyKey);
                    result = new ComparisonRuleNode(propertyName,
                        Resources.DefaultNamespace, schema, ComparisonOperator.Exact, value)
                    {
                        Scope = scope
                    };
                }
                else
                {
                    var or = new OrRuleNode()
                    {
                        Name = "OR"
                    };

                    foreach (var schema in schemas)
                    {
                        var propertyName = _model.GetPropertyName(schema, propertyKey);
                        if (propertyName != null)
                        {
                            or.Children.Add(new ComparisonRuleNode(propertyName,
                                Resources.DefaultNamespace, schema, ComparisonOperator.Exact, value)
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