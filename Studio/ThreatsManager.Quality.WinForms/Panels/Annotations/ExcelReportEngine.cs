using System;
using System.Collections.Generic;
using System.Drawing;
using PostSharp.Patterns.Contracts;
using Syncfusion.XlsIO;

namespace ThreatsManager.Quality.Panels.Annotations
{
    class ExcelReportEngine : IDisposable
    {
        private readonly ExcelEngine _engine = new ExcelEngine();
        private readonly IApplication _application;
        private readonly IWorkbook _workbook;
        private bool _noPages = true;
        private readonly Dictionary<int,int> _currentRows = new Dictionary<int, int>();

        public ExcelReportEngine()
        {
            _application = _engine.Excel;
            _workbook = _application.Workbooks.Create(1);
            _workbook.Version = ExcelVersion.Excel2016;
        }
        
        public void Dispose()
        {
            _engine?.Dispose();
        }

        public void AddHeader([Positive] int page, [NotNull] params string[] header)
        {
            var worksheet = _workbook.Worksheets[page];

            int c = 1;

            foreach (var item in header)
            {
                worksheet[1, c].Text = item;
                worksheet[1, c].ColumnWidth = 100;
                worksheet[1, c].CellStyle.Font.Bold = true;
                worksheet[1, c].WrapText = true;
                c++;
            }
        }

        public int AddPage([Required] string name)
        {
            int index = 0;

            if (_noPages)
            {
                var worksheet = _workbook.Worksheets[0];
                worksheet.Name = name;
                _noPages = false;
            }
            else
            {
                var worksheet = _workbook.Worksheets.Create(name);
                index = worksheet.Index;
            }

            return index;
        }

        public int AddRow([Positive] int page, [NotNull] params object[] cells)
        {
            var worksheet = _workbook.Worksheets[page];
            
            int r = 2;
            if (_currentRows.TryGetValue(page, out var index))
            {
                r += index;
            }
            else
            {
                index = 0;
            }

            int c = 1;
            foreach (var cell in cells)
            {
                worksheet[r, c].Value2 = cell;
                worksheet[r, c].WrapText = true;
                c++;
            }

            _currentRows[page] = index + 1;

            return r;
        }

        public void ColorCell([Positive] int page, [Positive] int row, [Positive] int column, 
            Color textColor, Color backColor)
        {
            var worksheet = _workbook.Worksheets[page];
            worksheet[row, column].CellStyle.Color = backColor;
            worksheet[row, column].CellStyle.Font.RGBColor = textColor;
        }

        public void Save([Required] string fileName)
        {
            var count = _workbook.Worksheets.Count;

            for (int i = 0; i < count; i++)
            {
                _workbook.Worksheets[i].UsedRange.AutofitColumns();
            }

            if (fileName.EndsWith(".csv", StringComparison.InvariantCultureIgnoreCase))
                _workbook.SaveAs(fileName, ",");
            else
                _workbook.SaveAs(fileName);
        }
    }
}
