using Interfaces.ReadOnly;

namespace Data
{
    public class DialogStrokeData : IReadOnlyDialogData
    {
        private NPCData _author;
        private string _text;
        private string _moodState;

        public DialogStrokeData(NPCData author, string text, string state)
        {
            this._author = author;
            this._text = text;
            this._moodState = state;
        }

        public NPCData Author
        {
            get => _author;
            set => _author = value;
        }

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        public string State
        {
            get => _moodState;
            set => _moodState = value;
        }
    }
}