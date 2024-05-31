using System;
using System.Collections.Generic;
using Enums;
using Managers.Notifiers;
using Randomizable;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class CharStateData
    {
        public string stateId;
        
        public Locker locker;
        public Informer informer;
        
        public CharStates state;
        
        public Transform transform;
        public List<string> dialogIds;
        public List<string> additionDialogIds;
        
        public List<string> lockerIdsForObject;
        public string informerIdForObject;
        
        public List<ItemDataForOperations> items;
        
        public string choiceIds;
        public List<DialogIdType> startDialogs;

        // FLAGS
        public bool dialogEndEqualStateEnd = false;
        public bool changeStateOnInform = false;
        public bool changeStateOnSuccessInteract = false;
        public bool startDialogOnStartState = false;
        public bool dialogBeforeAction = false;
        public bool showItemGivePanel = false;
        public bool needInform = false;

        public InformStatus[] informStatuses;
    }
}