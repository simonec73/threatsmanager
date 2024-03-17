using PostSharp.Patterns.Contracts;
using Syncfusion.XlsIO;
using System;
using System.Xml.Linq;

namespace ThreatsManager.Quality.Panels.Annotations
{
    class Line
    {
        public Line([NotNull] IRange range)
        {
            Id = range.Cells[0].Value;
            ObjectType = range.Cells[1].Value;
            Name = range.Cells[2].Value;
            AdditionalInfo = range.Cells[3].Value;
            AnnotationType = range.Cells[4].Value;
            AnnotationText = range.Cells[5].Value;
            AskedBy = range.Cells[6].Value;
            AskedOn = range.Cells[7].DateTime;
            AskedVia = range.Cells[8].Value;
            Answered = range.Cells[9].Boolean;
            RecordedAnswers = range.Cells[10].Value;
            NewAnswer = range.Cells[11].Value;
            NewAnswerBy = range.Cells[12].Value;
            NewAnswerOn = range.Cells[13].DateTime;
        }

        public Line(string id, string objectType, string name, string additionalInfo,
            string annotationType, string annotationText, string askedBy, DateTime askedOn,
            string askedVia, bool answered, string recordedAnswers, string newAnswer,
            string newAnswerBy, DateTime newAnswerOn)
        {
            Id = id;
            ObjectType = objectType;
            Name = name;
            AdditionalInfo = additionalInfo;
            AnnotationType = annotationType;
            AnnotationText = annotationText;
            AskedBy = askedBy;
            AskedOn = askedOn;
            AskedVia = askedVia;
            Answered = answered;
            RecordedAnswers = recordedAnswers;
            NewAnswer = newAnswer;
            NewAnswerBy = newAnswerBy;
            NewAnswerOn = newAnswerOn;
        }

        public string Id { get; private set; }
        public string ObjectType { get; private set; }
        public string Name { get; private set; }
        public string AdditionalInfo { get; private set; }
        public string AnnotationType { get; private set; }
        public string AnnotationText { get; private set; }
        public string AskedBy { get; private set; }
        public DateTime AskedOn { get; private set; }
        public string AskedVia { get; private set; }
        public bool Answered { get; private set; }
        public string RecordedAnswers { get; private set; }
        public string NewAnswer { get; private set; }
        public string NewAnswerBy { get; private set; }
        public DateTime NewAnswerOn { get; private set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(Id);
        public Guid Id0
        {
            get
            {
                var result = Guid.Empty;

                if (IsValid)
                {
                    if ((Id?.StartsWith("TT_") ?? false) ||
                        (Id?.StartsWith("TE_") ?? false))
                    {
                        var parts = Id.Split('_');
                        if (parts.Length == 3 && Guid.TryParseExact(parts[1], "N", out var parsed))
                        {
                            result = parsed;
                        }
                    }
                    else if (Guid.TryParseExact(Id, "N", out var parsed))
                    {
                        result = parsed;
                    }
                }

                return result;
            }
        }
        public Guid Id1
        {
            get
            {
                var result = Guid.Empty;

                if (IsValid)
                {
                    if ((Id?.StartsWith("TT_") ?? false) ||
                        (Id?.StartsWith("TE_") ?? false))
                    {
                        var parts = Id.Split('_');
                        if (parts.Length == 3 && Guid.TryParseExact(parts[2], "N", out var parsed))
                        {
                            result = parsed;
                        }
                    }
                }

                return result;
            }
        }
        public bool IsTopic => string.CompareOrdinal(AnnotationType, "Topic to be clarified") == 0;
        public bool HasNewAnswer => !string.IsNullOrWhiteSpace(NewAnswer);
    }
}
