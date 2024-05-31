using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using Managers;
using UnityEngine;

namespace Data
{
    [Serializable]
    public static class ItemOperations
    {
        private static ICharacterService _charService;
        
        private static DialogTransfer _dialogTransfer;
        private static DialogService _dialogService;
        
        private static AbilityLoader abilityLoader;
        private static ProofLoader proofLoader;
        private static SlotDataLoader slotDataLoader;
        private static UIMessageService _messageService;

        public static void Init(ICharacterService charService)
        {
            if (_charService != null) return;
            _charService = charService;

            _dialogTransfer = new DialogTransfer();
            _dialogService = new DialogService();
            
            abilityLoader = AbilityLoader.Instance;
            proofLoader = ProofLoader.Instance;
            slotDataLoader = SlotDataLoader.Instance;
            _messageService = UIMessageService.Instance;
        }
        
        public static bool GiveItem(
            string who, ItemDataForOperations item, bool showItemGetPanel = false)
        {
            var hasAbilData = abilityLoader.HasItem(item.id);
            if (hasAbilData)
            {
                return AbilityItemOperations(
                    who, item, OperationsType.Add, showItemGetPanel);
            }

            var hasSlotItemData = slotDataLoader.HasItem(item.id);
            if (hasSlotItemData)
            {
                return InventoryItemOperations(
                    who, item, OperationsType.Add, showItemGetPanel);
            }

            var hasProof = proofLoader.HasItem(item.id);
            if (hasProof)
            {
                return ProofItemOperations(
                    who, item, OperationsType.Add, showItemGetPanel);
            }

            return false;
        }

        public static bool RemoveItem(
            string who, ItemDataForOperations item)
        {
            var hasSlotItemData = slotDataLoader.HasItem(item.id);
            if (hasSlotItemData)
            {
                return InventoryItemOperations(who, item, OperationsType.Remove);
            }

            return false;
        }

        public static bool CheckItem(string who, ItemDataForOperations item)
        {
            var hasAbilData = abilityLoader.HasItem(item.id);
            if (hasAbilData)
            {
                return AbilityItemOperations(who, item, OperationsType.Check);
            }

            var hasSlotItemData = slotDataLoader.HasItem(item.id);
            if (hasSlotItemData)
            {
                return InventoryItemOperations(who, item, OperationsType.Check);
            }

            return false;
        }

        public static bool InventoryItemOperations(
            string who, ItemDataForOperations item,
            OperationsType type, bool needPlayAnim = false)
        {
            bool status = false;
            
            switch (type)
            {
                case OperationsType.Add:
                    _charService.TryAddToInventory(who, item.id,
                        item.amount, out int amountToAdd);
                    if (item.dialogIds.Count > 0) StartDialog(
                        who, item.dialogIds
                    );
                    if (needPlayAnim)
                    {
                        _messageService.StartItemGetAnim(
                            slotDataLoader.GetDataById(item.id).itemName
                        );
                    }
                    status = amountToAdd == 0;
                    break;
                
                case OperationsType.Check:
                    status = _charService
                        .GetInventory(_charService.PlayerName)
                        .HasEnoughAmount(item.id, item.amount);
                    break;
                
                case OperationsType.Remove:
                    IInventorySlots inventory = _charService
                        .GetInventory(_charService.PlayerName);
                    if (inventory.HasEnoughAmount(item.id, item.amount))
                    {
                        status = inventory
                            .TryRemoveItem(item.id, item.amount);
                    }
                    break;
            }
            
            return status;
        }
        
        public static bool ProofItemOperations(
            string who, ItemDataForOperations item,
            OperationsType type, bool needPlayAnim = false)
        {
            bool status = false;
            
            switch (type)
            {
                case OperationsType.Add:
                    status = _charService.TryAddProof(item.id);
                    if (item.dialogIds.Count > 0) StartDialog(
                        who, item.dialogIds
                    );
                    if (needPlayAnim)
                    {
                        _messageService.StartItemGetAnim(
                            proofLoader.GetDataById(item.id).title
                        );
                    }
                    break;
            }
            
            return status;
        }
        
        public static bool AbilityItemOperations(
            string who, ItemDataForOperations item,
            OperationsType type, bool needPlayAnim = false)
        {
            bool status = false;
            
            switch (type)
            {
                case OperationsType.Check:
                    status = _charService
                        .GetAbilities(_charService.PlayerName).Check(item.id);
                    break;
                
                case OperationsType.Add:
                    status = _charService.TryAddAbility(item.id);
                    if (item.dialogIds.Count > 0) StartDialog(
                        who, item.dialogIds
                    );
                    if (needPlayAnim)
                    {
                        _messageService.StartItemGetAnim(
                            abilityLoader.GetDataById(item.id).id
                        );
                    }
                    break;
            }

            return status;
        }
        
        private static void StartDialog(string who, List<string> ids)
        {
            _dialogService.SetIds(ids);
            _dialogTransfer.StartDialog(
                who, _dialogService, DialogType.Basic);
        }

    }
}