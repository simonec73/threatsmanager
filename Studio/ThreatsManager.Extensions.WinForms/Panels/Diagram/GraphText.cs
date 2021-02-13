using Northwoods.Go;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public class GraphText : GoText
    {
        private GraphLink _link;

        public GraphText([NotNull] GraphLink link)
        {
            _link = link;

            Text = "Flow";
            Alignment = Middle;
            Selectable = true;
            Editable = true;
            EditorStyle = GoTextEditorStyle.TextBox;
        }

        public override void OnGotSelection(GoSelection sel)
        {
            _link.RaiseSelectedEvent();
            DoBeginEdit(sel.View);
        }
    }
}