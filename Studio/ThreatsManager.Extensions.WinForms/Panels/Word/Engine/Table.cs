using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Extensions.Panels.Word.Engine
{
    internal class Table
    {
        private readonly List<Row> _rows = new List<Row>();

        private readonly float[] _columnWidth;
        private readonly float[] _defaultColumnWidth;

        public Table([NotNull] IEnumerable<string> header)
        {
            Header = header;
            _columnWidth = new float[header.Count()];
            _defaultColumnWidth = new float[header.Count()];
        }

        public IEnumerable<string> Header { get; private set; }

        public IEnumerable<Row> Rows => _rows;

        public int RowCount => _rows?.Count() ?? 0;

        public int ColumnCount => Header?.Count() ?? 0;

        public float TotalWidth => AutoWidth ? (_defaultColumnWidth?.Sum(x => x) ?? 0f) : _columnWidth?.Sum(x => x) ?? 0f;

        public bool AutoWidth => _columnWidth?.Any(x => x <= 0.0) ?? true;

        public float GetColumnWidth(int index)
        {
            float result = 0;

            if (index >= 0 && index < ColumnCount)
                result = AutoWidth ? _defaultColumnWidth[index] : _columnWidth[index];

            return result;
        }

        public void SetColumnWidth(int index, float width, float defaultWidth)
        {
            if (index >= 0 && index < ColumnCount)
            {
                _columnWidth[index] = width;
                _defaultColumnWidth[index] = defaultWidth;
            }
        }

        public void AddRow([NotNull] Row row)
        {
            _rows.Add(row);
        }
    }
}
