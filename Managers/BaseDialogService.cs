using System;
using System.Collections.Generic;
using Data;
using Enums;
using Interfaces.ReadOnly;
using UnityEngine;

namespace Managers
{
    public abstract class BaseDialogService
    {
        protected static IReadOnlyDialogs _dialogLoader;
        protected DialogStatus status;

        public BaseDialogService()
        {
            if (_dialogLoader == null)
            {
                _dialogLoader = new DialogLoader();
            }
            status = DialogStatus.End;
        }

        public List<Dialog> LoadDialogs(List<string> ids)
        {
            List<Dialog> dialogs = new List<Dialog>();
            foreach (string currDialogStringId in ids)
            {
                String[] stringIds = DataCleaner.CleanAndGetStrokesData(
                    currDialogStringId,
                    cleanType: DataSepparType.Main, splitType: DataSepparType.Main,
                    hasRange: true);
                List<int> tempIds = new List<int>(stringIds.Length);
                for (int i = 0; i < stringIds.Length; i++)
                {
                    tempIds.Insert(i, int.Parse(stringIds[i]));
                }
                Dialog tempDialog = new Dialog();
                List<DialogStrokeData> data = _dialogLoader.GetDataByIds(tempIds);
                tempDialog.DialogDatas = data;
                dialogs.Add(tempDialog);
            }
            return dialogs;
        }
        
        public Dialog LoadDialog(List<string> ids)
        {
            Dialog dialog = new Dialog();
            foreach (string currDialogStringId in ids)
            {
                String[] stringIds = DataCleaner.CleanAndGetStrokesData(
                    currDialogStringId,
                    cleanType: DataSepparType.Main, splitType: DataSepparType.Main,
                    hasRange: true);
                List<int> tempIds = new List<int>(stringIds.Length);
                for (int i = 0; i < stringIds.Length; i++)
                {
                    tempIds.Insert(i, int.Parse(stringIds[i]));
                }
                Dialog tempDialog = new Dialog();
                List<DialogStrokeData> data = _dialogLoader.GetDataByIds(tempIds);
                tempDialog.DialogDatas = data;
                dialog = tempDialog;
            }
            return dialog;
        }

        public DialogStatus GetStatus()
        {
            return status;
        }
    }
}