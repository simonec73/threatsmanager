using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreatsManager.Quality.Panels.Annotations
{
    class ExcelReaderEngine : IDisposable
    {
        private readonly ExcelEngine _engine = new ExcelEngine();
        private readonly IApplication _application;
        private readonly IWorkbook _workbook;
        private readonly IWorksheet _worksheet;
        private readonly string[] _fields = new[] {"ID", "Object Type", "Name", "Additional Info",
                        "Annotation Type", "Annotation Text", "Asked By", "Asked On", "Asked Via",
                        "Answered", "Recorded Answers", "New Answer", "New Answer By", "New Answer On"};

        public ExcelReaderEngine([Required] string fileName)
        {
            if (File.Exists(fileName))
            {
                _application = _engine.Excel;
                _workbook = _application.Workbooks.OpenReadOnly(fileName);
                if (_workbook.Worksheets.Count > 0)
                {
                    _worksheet = _workbook.Worksheets[0];
                }
            }
        }

        public IEnumerable<Line> ReadLines()
        {
            IEnumerable<Line> result = null;

            if (VerifyHeader())
            {
                int index = 1;
                var rows = _worksheet.Rows;
                IRange row;
                Line line;
                var list = new List<Line>();
                int max = rows.Length;

                do
                {
                    row = rows[index];
                    line = new Line(row);
                    if (line.IsValid)
                    {
                        list.Add(line);
                        index++;
                    }
                    else
                    {
                        break;
                    }
                } while (index < max);

                if (list.Count() > 0)
                    result = list.AsEnumerable();
            }

            return result;
        }

        public void Dispose()
        {
            _engine?.Dispose();
        }

        private bool VerifyHeader()
        {
            bool result = false;

            var rows = _worksheet.Rows;
            if (rows.Count() > 1)
            {
                var row = rows[0];
                result = true;

                for (int i = 0; i < _fields.Length; i++)
                {
                    var value = row.Cells[i].Value;
                    if (string.CompareOrdinal(value, _fields[i]) != 0)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
