using System.Collections.Generic;
using Interfaces.ReadOnly;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class DialogUIManager : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform dialog;
        [SerializeField] private RectTransform choiceDialog;

        [SerializeField] private TMP_Text author;
        [SerializeField] private TMP_Text text;
        private void Start()
        {
            onEndDialog();
        }

        public void Change(IReadOnlyDialogData data)
        {
            author.text = data.Author;
            text.text = data.Text;
        }

        public void Change(List<string> data)
        {
            
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
            canvas.enabled = false;
            author.text = "";
            text.text = "";
            return true;
        }
    }
}