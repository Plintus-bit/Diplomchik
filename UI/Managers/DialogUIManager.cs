using System.Collections.Generic;
using Interfaces.ReadOnly;
using UI;
using UnityEngine;

namespace Managers
{
    public class DialogUIManager : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private UITextWriter _textWriter;

        public DialogUI dialog;
        public ChoiceDialogUI choiceDialog;
        private void Start()
        {
            onEndDialog();
        }

        public void Change(IReadOnlyDialogData data)
        {
            choiceDialog.SetVisible(false);
            
            dialog.SetVisible(true);
            dialog.Change(data);
        }

        public void Change(List<string> data)
        {
            dialog.SetVisible(false);
            
            choiceDialog.SetVisible(true);
            choiceDialog.Change(data);
        }

        public bool OnStartDialog()
        {
            if (canvas.enabled) return false;
            canvas.enabled = true;
            return true;
        }

        public bool onEndDialog()
        {
            if (!canvas.enabled) return false;
            dialog.Clear();
            choiceDialog.SetVisible(false);
            dialog.SetVisible(false);
            choiceDialog.HideChoices();
            canvas.enabled = false;
            return true;
        }

        public bool IsEndWrite()
        {
            return _textWriter.IsEndWrite(dialog.text);
        }

        public void EndWrite()
        {
            _textWriter.EndWrite(dialog.text);
        }
    }
}