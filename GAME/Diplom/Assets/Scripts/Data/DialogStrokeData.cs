using Interfaces.ReadOnly;

namespace Data
{
    public class DialogStrokeData : IReadOnlyDialogData
    {
        private string _author;
        private string _text;

        public DialogStrokeData(string author, string text)
        {
            this._author = author;
            this._text = text;
        }

        public string Author
        {
            get => _author;
            set => _author = value;
        }

        public string Text
        {
            get => _text;
            set => _text = value;
        }
    }
}