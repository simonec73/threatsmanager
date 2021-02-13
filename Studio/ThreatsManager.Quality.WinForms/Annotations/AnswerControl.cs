using System;
using System.Windows.Forms;

namespace ThreatsManager.Quality.Annotations
{
    public partial class AnswerControl : UserControl
    {
        private AnnotationAnswer _answer;

        public AnswerControl()
        {
            InitializeComponent();
        }

        public AnnotationAnswer Answer
        {
            get => _answer;

            set
            {
                _answer = value;
                _text.Text = _answer?.Text;
                _answeredBy.Text = _answer?.AnsweredBy;
                _answeredOn.Value = _answer?.AnsweredOn ?? DateTime.MinValue;
                _answeredVia.Text = _answer?.AnsweredVia;
            }
        }

        private void _text_TextChanged(object sender, EventArgs e)
        {
            if (_answer != null)
                _answer.Text = _text.Text;
        }

        private void _answeredBy_TextChanged(object sender, EventArgs e)
        {
            if (_answer != null)
                _answer.AnsweredBy = _answeredBy.Text;
        }

        private void _answeredOn_ValueChanged(object sender, EventArgs e)
        {
            if (_answer != null)
                _answer.AnsweredOn = _answeredOn.Value;
        }

        private void _answeredVia_TextChanged(object sender, EventArgs e)
        {
            if (_answer != null)
                _answer.AnsweredVia = _answeredVia.Text;
        }

        private void _answeredOn_ButtonCustomClick(object sender, EventArgs e)
        {
            _answeredOn.Value = DateTime.Now;
        }
    }
}
