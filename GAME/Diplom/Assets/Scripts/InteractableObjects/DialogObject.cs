using System;
using System.Collections.Generic;
using Data;
using Enums;
using Managers;
using UnityEngine;

namespace InteractableObjects
{
    public class DialogObject : BasicObject
    {
        [SerializeField] private List<string> ids;
        [SerializeField] private DialogUIManager dialogUIManager;
        private DialogService _dialogService;
        
        protected override void Initialize()
        {
            dialogUIManager = FindObjectOfType<DialogUIManager>();
            _dialogService = new DialogService(ids);
        }

        public override bool Interact(string who)
        {
            NextDialog();
            return IsInteractable;
        }

        public override void SetIconNames()
        {
            IconName = "DialogIcon";
        }

        public void NextDialog()
        {
            DialogStrokeData currDialog = _dialogService.NextDialog();
            if (currDialog == null)
            {
                dialogUIManager.onEndDialog();
            }
            else
            {
                DialogStatus status = _dialogService.GetStatus();
                if (status == DialogStatus.Process)
                {
                    dialogUIManager.Change(currDialog);
                } else if (status == DialogStatus.Start)
                {
                    dialogUIManager.Change(currDialog);
                    dialogUIManager.OnStartDialog();
                }
            }
        }
    }
}