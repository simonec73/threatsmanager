using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Recording.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class UndoRedoOperationsDialog : Form
    {
        public UndoRedoOperationsDialog()
        {
            InitializeComponent();
        }

        private void _refresh_Click(object sender, EventArgs e)
        {
            LoadOperations();
        }

        private void UndoRedoOperationsDialog_Load(object sender, EventArgs e)
        {
            LoadOperations();
        }

        private void LoadOperations()
        {
            _undo.Text = GetOperationsText(UndoRedoManager.UndoOperations, out var undoFirst, out var undoTotal);
            _countUndoFirst.Text = undoFirst.ToString();
            _countUndo.Text = undoTotal.ToString();
            _redo.Text = GetOperationsText(UndoRedoManager.RedoOperations, out var redoFirst, out var redoTotal);
            _countRedoFirst.Text = redoFirst.ToString();
            _countRedo.Text = redoTotal.ToString();
        }

        private string GetOperationsText(IEnumerable<Operation> operations, out int firstLevel, out int totalCount)
        {
            string result = null;

            totalCount = 0;

            if (operations?.Any() ?? false)
            {
                firstLevel = operations.Count();

                var builder = new StringBuilder();

                uint level = 0;

                foreach (var operation in operations)
                {
                    if (builder.Length > 0)
                        builder.AppendLine("---");
                    LoadOperation(operation, builder, level, ref totalCount);
                }

                result = builder.ToString();
            }
            else
                firstLevel = 0;

            return result;
        }

        private void LoadOperation(Operation operation, StringBuilder builder, uint level, ref int count)
        {
            count++;

            var prefix = GetPrefix(level);

            string operationName = null;

            try
            {
                operationName = operation.Name;
            }
            catch
            {
            }

            if (string.IsNullOrWhiteSpace(operationName))
                operationName = "Unknown";

            builder.AppendLine($"{prefix}{operationName}");
            builder.AppendLine($"{prefix}Operation kind: {operation.OperationKind}");

            if (operation is ICollectionOperation opColl)
            {
                builder.AppendLine($"{prefix}Collection type: {opColl.Collection.GetType().FullName}");
                if (opColl.OldItem != null)
                {
                    builder.AppendLine($"{prefix}OldItem: {opColl.OldItem}");
                    builder.AppendLine($"{prefix}OldItem type: {opColl.OldItem.GetType().FullName}");
                }
                if (opColl.NewItem != null)
                {
                    builder.AppendLine($"{prefix}NewItem: {opColl.NewItem}");
                    builder.AppendLine($"{prefix}NewItem type: {opColl.NewItem.GetType().FullName}");
                }
            }

            if (operation is IDictionaryOperation opDict)
            {
                builder.AppendLine($"{prefix}Dictionary type: {opDict.Dictionary.GetType().FullName}");
                builder.AppendLine($"{prefix}Key: {opDict.Key}");
                if (opDict.OldValue != null)
                {
                    builder.AppendLine($"{prefix}OldValue: {opDict.OldValue}");
                    builder.AppendLine($"{prefix}OldValue type: {opDict.OldValue.GetType().FullName}");
                }
                if (opDict.NewValue != null)
                {
                    builder.AppendLine($"{prefix}NewValue: {opDict.NewValue}");
                    builder.AppendLine($"{prefix}NewValue type: {opDict.NewValue.GetType().FullName}");
                }
            }

            if (operation is IHashSetOperation opHash)
            {
                builder.AppendLine($"{prefix}HashSet type: {opHash.HashSet.GetType()}");
                if (opHash.OldItem != null)
                {
                    builder.AppendLine($"{prefix}OldItem: {opHash.OldItem}");
                    builder.AppendLine($"{prefix}OldItem type: {opHash.OldItem.GetType().FullName}");
                }
                if (opHash.NewItem != null)
                {
                    builder.AppendLine($"{prefix}NewItem: {opHash.NewItem}");
                    builder.AppendLine($"{prefix}NewItem type: {opHash.NewItem.GetType().FullName}");
                }
            }

            if (operation is AttachObjectToRecorderOperation opAttach)
            {
                builder.AppendLine($"{prefix}Object: {opAttach.Object}");
                builder.AppendLine($"{prefix}Object type: {opAttach.Object.GetType().FullName}");
            }

            if (operation is DetachObjectFromRecorderOperation opDetach)
            {
                builder.AppendLine($"{prefix}Object: {opDetach.Object}");
                builder.AppendLine($"{prefix}Object type: {opDetach.Object.GetType().FullName}");
            }

            if (operation is IFieldOperation opField)
            {
                builder.AppendLine($"{prefix}Target: {opField.Target}");
                builder.AppendLine($"{prefix}Target type: {opField.Target.GetType().FullName}");
                builder.AppendLine($"{prefix}Location: {opField.Location.Name}");
                if (opField.OldValue != null)
                {
                    builder.AppendLine($"{prefix}OldValue: {opField.OldValue}");
                    builder.AppendLine($"{prefix}OldValue type: {opField.OldValue.GetType().FullName}");
                }
                if (opField.NewValue != null)
                {
                    builder.AppendLine($"{prefix}NewValue: {opField.NewValue}");
                    builder.AppendLine($"{prefix}NewValue type: {opField.NewValue.GetType().FullName}");
                }
            }

            if (operation is CompositeOperation opComposite)
            {
                LoadCompositeOperation(opComposite, builder, level, ref count);
            }
            else
                builder.AppendLine();
        }

        private void LoadCompositeOperation([NotNull] CompositeOperation operation, [NotNull] StringBuilder builder, uint level, ref int count)
        {
            var prefix = GetPrefix(level);

            builder.AppendLine($"{prefix}Composite Operation: {operation.Name}");
            builder.AppendLine();

            var field = operation.GetType().GetField("subOperations", BindingFlags.NonPublic | BindingFlags.Instance);
            var operations = field.GetValue(operation) as List<Operation>;
            if (operations?.Any() ?? false)
            {
                foreach (var op in operations)
                    LoadOperation(op, builder, level + 1, ref count);
            }
        }

        private string GetPrefix(uint level)
        {
            var space = level > 0 ? new string(' ', ((int)level) * 2) : "";
            return $"{space}[{level}] ";
        }

        private void _close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _previousUndo_Click(object sender, EventArgs e)
        {
            var current = GetTopLineIndex(_undo);
            var previous = GetPreviousTopLevel(_undo, current);
            if (previous >= 0)
            {
                SetTopIndex(_undo, previous);
            }
        }

        private void _nextUndo_Click(object sender, EventArgs e)
        {
            var current = GetTopLineIndex(_undo);
            var next = GetNextTopLevel(_undo, current);
            if (next >= 0)
            {
                SetTopIndex(_undo, next);
            }
        }

        private void _previousRedo_Click(object sender, EventArgs e)
        {
            var current = GetTopLineIndex(_redo);
            var next = GetPreviousTopLevel(_redo, current);
            if (next >= 0)
            {
                SetTopIndex(_redo, next);
            }
        }

        private void _nextRedo_Click(object sender, EventArgs e)
        {
            var current = GetTopLineIndex(_redo);
            var next = GetNextTopLevel(_redo, current);
            if (next >= 0)
            {
                SetTopIndex(_redo, next);
            }
        }

        private int GetTopLineIndex(RichTextBox textBox)
        {
            int firstVisibleChar = textBox.GetCharIndexFromPosition(new System.Drawing.Point(0, 0));
            return textBox.GetLineFromCharIndex(firstVisibleChar);
        }

        private void SetTopIndex(RichTextBox textBox, int pos)
        {
            textBox.SelectionStart = pos;
            textBox.ScrollToCaret();
        }

        private int GetNextTopLevel(RichTextBox textBox, int currentLine)
        {
            var pos = textBox.GetFirstCharIndexFromLine(currentLine + 1);
            return textBox.Find("---", pos + 1, RichTextBoxFinds.NoHighlight);
        }

        private int GetPreviousTopLevel(RichTextBox textBox, int currentLine)
        {
            int result = -1;

            if (currentLine > 0)
            {
                var pos = textBox.GetFirstCharIndexFromLine(currentLine);
                result = textBox.Find("---", 0, pos - 1, RichTextBoxFinds.NoHighlight | RichTextBoxFinds.Reverse);
                if (result == -1)
                    result = 0;
            }

            return result;
        }
    }
}
