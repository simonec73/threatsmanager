using System.Collections.Generic;

namespace ThreatsManager.MsTmt.Model.AutoThreatGen
{
    class MyVisitor : TmtBaseVisitor<object>
    {
        public List<string> Lines = new List<string>();

        public override object VisitBooleanExpression(TmtParser.BooleanExpressionContext context)
        {
            string line = $"{context.left?.GetText()} {context.op?.GetText()} {context.right?.GetText()}";
            Lines.Add(line);

            Visit(context.left);
            Visit(context.right);

            return line;
        }

        public override object VisitFlowExpression(TmtParser.FlowExpressionContext context)
        {
            string line = $"{context.left?.Text} {context.op?.GetText()} {context.right?.Text}";
            Lines.Add(line);

            return line;
        }

        public override object VisitNotExpression(TmtParser.NotExpressionContext context)
        {
            string line = $"NOT {context.expression()?.GetText()}";
            Lines.Add(line);

            Visit(context.expression());

            return line;
        }

        public override object VisitParentExpression(TmtParser.ParentExpressionContext context)
        {
            string line = $"( {context.expression()?.GetText()} )";
            Lines.Add(line);

            Visit(context.expression());

            return line;
        }

        public override object VisitSubjectExpression(TmtParser.SubjectExpressionContext context)
        {
            string line = $"{context.left?.GetText()} {context.op?.Text} {context.right?.Text}";
            Lines.Add(line);

            return line;
        }

        public override object VisitPropertyExpression(TmtParser.PropertyExpressionContext context)
        {
            string line = $"{context.left?.GetText()} {context.op?.Text} {context.right?.Text}";
            Lines.Add(line);

            return line;
        }

        private void Visit(TmtParser.ExpressionContext context)
        {
            if (context is TmtParser.BooleanExpressionContext booleanExpressionContext)
            {
                VisitBooleanExpression(booleanExpressionContext);
            }
            else if (context is TmtParser.FlowExpressionContext flowExpressionContext)
            {
                VisitFlowExpression(flowExpressionContext);
            }
            else if (context is TmtParser.NotExpressionContext notExpressionContext)
            {
                VisitNotExpression(notExpressionContext);
            }
            else if (context is TmtParser.ParentExpressionContext parentExpressionContext)
            {
                VisitParentExpression(parentExpressionContext);
            }
            else if (context is TmtParser.SubjectExpressionContext subjectExpressionContext)
            {
                VisitSubjectExpression(subjectExpressionContext);
            }
            else if (context is TmtParser.PropertyExpressionContext propertyExpressionContext)
            {
                VisitPropertyExpression(propertyExpressionContext);
            }
        }
    }
}