using System.Collections.Generic;
using Enums;
using Managers;
using Randomizable;
using UnityEngine;

namespace InteractableObjects
{
    public class ChoiceDialogObject : DialogObject
    {
        [SerializeField] private string choiceIds;
        [SerializeField] private List<DialogIdType> startDialogs;
        protected override void Initialize()
        {
            _dialogService = new ChoiceDialogService(choiceIds, ids, startDialogs);
            _dialogTransfer = new DialogTransfer(OnEndDialog);
            type = DialogType.Choice;
        }

    }
}