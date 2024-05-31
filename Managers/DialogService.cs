using System.Collections.Generic;
using Data;
using Enums;
using UnityEngine;

namespace Managers
{
    public class DialogService : BaseDialogService
    {
        protected List<string> _ids;
        
        protected List<Dialog> _dialogs;
        protected int _currDialogId;
        protected Dialog _currDialog;

        protected int _indexStart;
        protected int _indexEnd;

        public readonly int DialogEndIndex = -1;
        public int IndexStart => _indexStart;
        public int IndexEnd => _indexEnd;

        public DialogService(List<string> ids = null)
        {
            _dialogs = new List<Dialog>();
            _currDialog = null;
            _currDialogId = -1;
            _ids = ids;
            ClearLoop();
        }

        public void SetIds(List<string> ids)
        {
            if (ids == null) return;
            Clear();
            _ids = ids;
            _dialogs = LoadDialogs(_ids);
            status = DialogStatus.End;
            
            ClearLoop();
        }
        
        public virtual DialogStrokeData NextDialog()
        {
            if (_ids == null) return null;
            status = DialogStatus.Process;
            if (_dialogs == null || _dialogs.Count == 0)
            {
                _dialogs = LoadDialogs(_ids);
            }
            if (_currDialogId < 0)
            {
                ChangeCurrentDialogId();
            }
            if (_currDialog == null)
            {
                ChangeCurrentDialog();
                status = DialogStatus.Start;
            }
            int currDialogBubbleId = _currDialog.NextDialog();
            if (currDialogBubbleId == _currDialog.DialogCount)
            {
                ChangeCurrentDialogId();
                status = DialogStatus.End;
                return null;
            }
            DialogStrokeData currStrokeData = _currDialog.GetCurrentDialog();
            return currStrokeData;
        }

        public void ChangeCurrentDialogId()
        {
            if (_currDialogId < 0)
            {
                _currDialogId = _indexStart;
            }
            else
            {
                _currDialogId += 1;
            }
            
            if (_currDialogId > _indexEnd)
            {
                _currDialogId = -1;
            }
            _currDialog = null;
        }
        
        public void ChangeCurrentDialogId(int id)
        {
            _currDialogId = id;
            _currDialog = null;
        }

        public void ChangeCurrentDialog()
        {
            if (_currDialogId < 0) _currDialogId = _indexStart;
            _currDialog = _dialogs[_currDialogId];
        }

        public bool IsEmpty()
        {
            return _ids == null || _ids.Count == 0;
        }
        
        public void SetLoop(int indexStart, int indexEnd = -1)
        {
            _indexStart = indexStart;
            if (indexEnd < indexStart)
            {
                _indexEnd = GetDialogsCount() - 1;
            }
            else
            {
                _indexEnd = indexEnd;
            }

            ChangeCurrentDialogId(_indexStart);
        }

        public int GetCurrDialogIndex()
        {
            return _currDialogId;
        }

        public int GetDialogsCount()
        {
            return _ids.Count;
        }
        
        public void ClearLoop()
        {
            _indexStart = 0;
            if (_ids == null) return;
            _indexEnd = _ids.Count - 1;
        }
        
        public void Clear()
        {
            if (_dialogs != null)
            {
                _dialogs.Clear();
            }
            else
            {
                _dialogs = new List<Dialog>();
            }
            _currDialog = null;
            _currDialogId = -1;
            _ids = null;
        }
    }
}