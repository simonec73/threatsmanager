using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.PropertySchemaList
{
    [ToolboxItem(false)]
    public class GridTokenEditControl : TokenEditor, IGridCellEditControl
    {
        #region Private member variables

        private Bitmap _editorCellBitmap;

        private bool _valueChanged;

        #endregion

        public GridTokenEditControl()
        {
            EditTextBox.KeyPress += EditTextBoxOnKeyPress;
            Separators.Add(";");
            ValidateToken += OnValidateToken;
            SelectedTokensChanged += OnSelectedTokensChanged;
        }

        private void EditTextBoxOnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                e.Handled = true;
                e.KeyChar = ';';
            }
        }

        private void OnValidateToken(object sender, ValidateTokenEventArgs ea)
        {
            ea.IsValid = !(SelectedTokens?.Contains(ea.Token) ?? false);
        }

        private void OnSelectedTokensChanged(object sender, EventArgs e)
        {
            if (SuspendUpdate == false)
            {
                EditorCell.Value = SelectedTokens?.Select(x => x.Value).TagConcat();

                EditorCell.EditorValueChanged(this);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            EditorCell.PaintEditorBackground(e, this);
        }

        public virtual string GetValue(object value)
        {
            GridPanel panel = EditorCell.GridPanel;

            if (value == null ||
                (panel.NullValue == NullValue.DBNull && value == DBNull.Value))
            {
                return ("");
            }

            if (EditorCell.IsValueExpression)
                value = EditorCell.GetExpValue((string)value);

            return (Convert.ToString(value));
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void SetValue(string s)
        {
            SuspendUpdate = true;
            SelectedTokens.Clear();
            var tags = s?.TagSplit();
            if (tags?.Any() ?? false)
            {
                foreach (var tag in tags)
                {
                    if (!string.IsNullOrWhiteSpace(tag))
                        SelectedTokens.Add(new EditToken(tag));
                }
            }
            SuspendUpdate = false;
        }

        public bool CanInterrupt => true;

        public CellEditMode CellEditMode => CellEditMode.Modal;

        public GridCell EditorCell { get; set; }

        public Bitmap EditorCellBitmap
        {
            get => (_editorCellBitmap);

            set
            {
                _editorCellBitmap?.Dispose();

                _editorCellBitmap = value;
            }
        }

        public virtual string EditorFormattedValue => (string) EditorValue;

        public EditorPanel EditorPanel { get; set; }

        public virtual object EditorValue
        {
            get => EditorCell.Value;
            set => SetValue(GetValue(value));
        }

        public virtual bool EditorValueChanged
        {
            get => (_valueChanged);

            set
            {
                if (_valueChanged != value)
                {
                    _valueChanged = value;

                    if (value)
                        EditorCell.SetEditorDirty(this);
                }
            }
        }

        public virtual Type EditorValueType => (typeof(string));

        public virtual StretchBehavior StretchBehavior { get; set; } = StretchBehavior.HorizontalOnly;

        public bool SuspendUpdate { get; set; }

        public virtual ValueChangeBehavior ValueChangeBehavior => ValueChangeBehavior.InvalidateLayout;

        public virtual void InitializeContext(GridCell cell, CellVisualStyle style)
        {
            EditorCell = cell;

            SetValue(GetValue(EditorCell.Value));

            Enabled = (EditorCell.ReadOnly == false);

            if (style != null)
            {
                Font = style.Font;
                ForeColor = style.TextColor;
            }

            _valueChanged = false;
        }

        public virtual Size GetProposedSize(Graphics g,
            GridCell cell, CellVisualStyle style, Size constraintSize)
        {
            return (Size);
        }
        public virtual bool BeginEdit(bool selectAll)
        {
            return (false);
        }

        public virtual bool EndEdit()
        {
            return (false);
        }

        public virtual bool CancelEdit()
        {
            return (false);
        }

        public virtual void CellRender(Graphics g)
        {
            EditorCell.CellRender(this, g);
        }

        public virtual void CellKeyDown(KeyEventArgs e)
        {
        }

        public virtual bool WantsInputKey(Keys key, bool gridWantsKey)
        {
            return !gridWantsKey || key == Keys.Enter;
        }

        public virtual void OnCellMouseMove(MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        public virtual void OnCellMouseEnter(EventArgs e)
        {
            OnMouseEnter(e);
        }

        public virtual void OnCellMouseLeave(EventArgs e)
        {
            Refresh();
        }

        public virtual void OnCellMouseUp(MouseEventArgs e)
        {
            OnMouseUp(e);
        }

        public virtual void OnCellMouseDown(MouseEventArgs e)
        {
            OnMouseDown(e);
        }
    }
}
