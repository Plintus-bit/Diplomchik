using System;
using System.Collections.Generic;
using Data;
using Enums;
using Interfaces.ReadOnly;

namespace Managers
{
    public class DialogService
    {
        private static IReadOnlyDialogs _dialogLoader;
        
        private List<Dialog> _dialogs;
        private List<string> ids;
        private int _currDialogId;
        private Dialog _currDialog;
        private int _dialogCount;
        private DialogStatus _status;

        public DialogService(List<string> ids)
        {
            if (_dialogLoader == null)
            {
                _dialogLoader = new DialogLoader();
            }
            _dialogs = new List<Dialog>();
            _currDialog = null;
            _currDialogId = -1;
            this.ids = ids;
            _status = DialogStatus.End;
        }
        
        public virtual DialogStrokeData NextDialog()
        {
            _status = DialogStatus.Process;
            if (_dialogs == null || _dialogs.Count == 0)
            {
                LoadDialogs();
            }
            if (_currDialogId < 0)
            {
                ChangeCurrentDialogId();
            }
            if (_currDialog == null)
            {
                ChangeCurrentDialog();
                _status = DialogStatus.Start;
            }
            int currDialogBubbleId = _currDialog.NextDialog();
            if (currDialogBubbleId == _currDialog.DialogCount)
            {
                ChangeCurrentDialogId();
                _status = DialogStatus.End;
                return null;
            }
            DialogStrokeData currStrokeData = _currDialog.GetCurrentDialog();
            return currStrokeData;
        }

        private void ChangeCurrentDialogId()
        {
            _currDialogId += 1;
            if (_currDialogId >= _dialogCount)
            {
                _currDialogId = -1;
            }
            _currDialog = null;
        }

        private void ChangeCurrentDialog()
        {
            _currDialog = _dialogs[_currDialogId];
        }
        
        public void LoadDialogs()
        {
            foreach (string currDialogStringId in ids)
            {
                String[] stringIds = currDialogStringId.Split(";");
                List<int> tempIds = new List<int>(stringIds.Length);
                for (int i = 0; i < stringIds.Length; i++)
                {
                    tempIds.Insert(i, int.Parse(stringIds[i]));
                }
                Dialog tempDialog = new Dialog();
                List<DialogStrokeData> data = _dialogLoader.GetDataByIds(tempIds);
                tempDialog.DialogDatas = data;
                _dialogs.Add(tempDialog);
            }
            _dialogCount = _dialogs.Count;
        }

        public DialogStatus GetStatus()
        {
            return this._status;
        }
    }
}