using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class Dialog
    {
        private List<DialogStrokeData> _dialogDatas;
        private int _currDialogId;
        private int _dialogCount;
        
        public int DialogCount => _dialogCount;

        public List<DialogStrokeData> DialogDatas
        {
            get => _dialogDatas;
            set
            {
                _dialogDatas = value;
                _dialogCount = value.Count;
            }
        }

        public Dialog()
        {
            _dialogDatas = null;
            _dialogCount = 0;
            _currDialogId = -1;
        }

        public Dialog(DialogStrokeData sentence)
        {
            _dialogDatas = new List<DialogStrokeData>();
            _dialogDatas.Add(sentence);
            _dialogCount = 1;
            _currDialogId = -1;
        }

        public int NextDialog()
        {
            _currDialogId += 1;
            if (_currDialogId < 0)
            {
                _currDialogId = 0;
            } else if (_currDialogId >= _dialogCount)
            {
                _currDialogId = -1;
                return _dialogCount;
            }
            return _currDialogId;
        }

        public DialogStrokeData GetCurrentDialog()
        {
            return _dialogDatas[_currDialogId];
        }
    }
}