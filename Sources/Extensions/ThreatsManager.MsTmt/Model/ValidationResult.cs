using PostSharp.Patterns.Contracts;

namespace ThreatsManager.MsTmt.Model
{
    public class ValidationResult
    {
        public ValidationResult([Required] string text, bool isError = false)
        {
            Text = text;
            IsBlockingError = isError;
        }

        public string Text { get; private set; }
        public bool IsBlockingError { get; private set; }
    }
}