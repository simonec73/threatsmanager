using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class WordReportDefinitions
    {
        [JsonProperty("reports", ItemTypeNameHandling = TypeNameHandling.Objects, TypeNameHandling = TypeNameHandling.None)]
        [Reference]
        private AdvisableCollection<WordReportDefinition> _reports { get; set; }

        public bool HasReports => _reports?.Any() ?? false;

        public IEnumerable<WordReportDefinition> Reports => _reports?.Cast<WordReportDefinition>();

        public void AddReport([NotNull] WordReportDefinition report)
        {
            using (var scope = UndoRedoManager.OpenScope("Add Report"))
            {
                if (_reports == null)
                    _reports = new AdvisableCollection<WordReportDefinition>();
                _reports.Add(report);
                scope?.Complete();
            }
        }

        public void AddReports([NotNull] IEnumerable<WordReportDefinition> reports)
        {
            if (reports?.Any() ?? false)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Reports"))
                {
                    if (_reports == null)
                        _reports = new AdvisableCollection<WordReportDefinition>();
                    _reports.AddRange(reports);
                    scope?.Complete();
                }
            }
        }

        public bool RemoveReport([NotNull] WordReportDefinition report)
        {
            var result = false;

            using (var scope = UndoRedoManager.OpenScope("Remove Report"))
            {
                result = _reports.Remove(report);
                scope.Complete();
            }

            return result;
        }
    }
}
