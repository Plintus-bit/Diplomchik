using System;
using System.Collections.Generic;
using Data;
using Enums;
using Randomizable;
using UnityEngine;

namespace Managers
{
    public class ChoiceDialogService : DialogService
    {
        private List<string> _choices;
        private List<DialogIdType> _startDialogVariables;
        private int dialogChoice;
        private static int dialogOff = -1;

        public ChoiceDialogService(string choicesIds,
            List<string> ids, List<DialogIdType> startDialogs = null)
        {
            Set(choicesIds, ids, startDialogs);
        }

        public ChoiceDialogService()
        {
            _currDialog = null;
            _currDialogId = -1;
            _currDialogId = dialogOff;
        }

        public void Set(string choicesIds,
            List<string> ids, List<DialogIdType> startDialogs = null)
        {
            _choices = LoadChoices(choicesIds);
            _dialogs = LoadDialogs(ids);
            if (startDialogs != null)
            {
                _startDialogVariables = startDialogs;
                foreach (var item in _startDialogVariables)
                {
                    item.dialog = LoadDialog(item.dialogIds);
                }
            }
            _currDialog = null;
            _currDialogId = -1;
            _ids = ids;
            _currDialogId = dialogOff;
            if (_choices.Count != _dialogs.Count) throw new IndexOutOfRangeException();
            dialogChoice = _choices.Count;   
        }
        
        public List<string> LoadChoices(string choicesIds)
        {
            String[] stringIds = DataCleaner.CleanAndGetStrokesData(
                choicesIds,
                cleanType: DataSepparType.Main,
                splitType: DataSepparType.Main,
                hasRange: true);
            List<int> tempIds = new List<int>(stringIds.Length);
            for (int i = 0; i < stringIds.Length; i++)
            {
                tempIds.Insert(i, int.Parse(stringIds[i]));
            }

            return _dialogLoader.GetChoiceDataByIds(tempIds);
        }

        public override DialogStrokeData NextDialog()
        {
            status = DialogStatus.Process;
            if (_currDialogId == dialogOff)
            {
                ChangeCurrentDialog(dialogChoice);
            }
            if (_currDialog == null) return null;
            int currDialogBubbleId = _currDialog.NextDialog();
            if (currDialogBubbleId == _currDialog.DialogCount)
            {
                ChangeCurrentDialog(dialogChoice);
            }
            else
            {
                return _currDialog.GetCurrentDialog();
            }
            return null;
        }

        public void EndDialog()
        {
            ChangeCurrentDialog(dialogOff);
        }

        public void ChangeCurrentDialog(int id)
        {
            if (id == dialogOff)
            {
                status = DialogStatus.End;
                _currDialog = null;
            } else if (id == dialogChoice)
            {
                if (_currDialogId == dialogOff && _startDialogVariables != null)
                {
                    status = DialogStatus.Start;
                    if (_startDialogVariables.Count == 1)
                    {
                        _currDialog = _startDialogVariables[0].GetDialog();
                    } else _currDialog = Randomizer<DialogIdType>
                        .Randomize(_startDialogVariables).GetDialog();
                }
                else {
                    status = DialogStatus.Choice;
                    _currDialog = null;
                }
            }
            else
            {
                status = DialogStatus.Process;
                _currDialog = _dialogs[id];
            }
            _currDialogId = id;
        }

        public List<string> GetChoices()
        {
            return _choices;
        }
        
    }
}