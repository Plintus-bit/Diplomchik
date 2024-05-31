using System.Collections.Generic;
using Data;
using UI;
using UnityEngine;

namespace Brainteasers
{
    public class BedTableBrainteaser : BaseBrainTeaser
    {
        [SerializeField] private List<BedTableBrainteaserItemUI> _items;
        [SerializeField] private List<AnswerStates<bool>> _stages;
        [SerializeField] private ItemDataForOperations _reward;
        
        private AnswerStates<bool> _currentStage;
        private int _stageIndex;

        protected override void Initialize()
        {
            base.Initialize();
            
            if (_stages.Count > 0)
            {
                OnStartStage(0);
            }
            if (ids.Count != 0)
                _dialogTransfer.SetAction(OnEndDialog);
        }

        protected void OnStartStage(int index)
        {
            _currentStage = _stages[index];
            _currentStage.Init();
            _stageIndex = index;
            var startValues = _currentStage.StartValues;

            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Init(this, startValues[i], i);
            }
        }
        
        public override void OnUIAction(int index)
        {
            if (_items.Count == 0) return;
            bool newState = _items[index].Change();
            bool isCorrect = _currentStage.CheckAnswer(index, newState);
            if (isCorrect && _currentStage.CheckIsWin())
            {
                if (_stageIndex == _stages.Count - 1)
                {
                    OnSolve();
                }
            }
        }

        public void OnSolve()
        {
            _stages.Clear();
            _items.Clear();
            brainTeaserUI.TurnOff();
            if (!StartDialog(_charService.PlayerName))
            {
                OnEndDialog();
            }
        }

        public override void OnEndDialog()
        {
            Close();
            _locker.Unlock(true);
            ItemOperations.GiveItem(
                _charService.PlayerName,
                _reward, true);
            DestroyObject();
        }
    }
}