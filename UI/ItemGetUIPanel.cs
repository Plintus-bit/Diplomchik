using System;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ItemGetUIPanel : MonoBehaviour
    {
        private Vector3 startPosition;
        private bool _isAnimPlay;
        private Action OnAnimEndPlay;
        
        // for animation
        private Vector3 sky_1_startPos;
        private Vector3 sky_2_startPos;
        private Vector3 youGot_startPos;
        private Vector3 itemPanel_startPos;

        private UIMessageService _whoCall;

        // for animation
        public CanvasGroup group;

        public Transform sky_1;
        public Transform sky_2;
        public Transform youGotItemPanel;
        public Transform itemPanel;
        public TMP_Text itemTitlePlace;

        public float deltaYForAnim = 360f;
        public float timeForStay = 2.2f;
        
        public bool IsAnimPlayed => _isAnimPlay;
        
        private void Awake()
        {
            sky_1_startPos = sky_1.localPosition;
            sky_2_startPos = sky_2.localPosition;
            youGot_startPos = youGotItemPanel.localPosition;
            itemPanel_startPos = itemPanel.localPosition;
            
            startPosition = transform.localPosition;
        }

        private void PlayAnim()
        {
            _isAnimPlay = true;
            transform.localPosition = startPosition;
            // group.alpha = 1f;

            // transform
            //     .LeanMoveLocal(transform.localPosition
            //                    + Vector3.down * 270f, 0.7f)
            //     .setEase(LeanTweenType.easeOutBounce);
            // group.LeanAlpha(0f, 1.2f)
            //     .setDelay(2.5f)
            //     .setOnComplete(SetAnimPlayed);

            sky_1
                .LeanMoveLocal(sky_1_startPos
                               + Vector3.down * deltaYForAnim, 0.4f)
                .setEaseOutCubic();
            sky_2
                .LeanMoveLocal(sky_2_startPos
                               + Vector3.down * deltaYForAnim, 0.4f)
                .setEaseOutCubic()
                .setDelay(0.05f);
            itemPanel
                .LeanMoveLocal(itemPanel_startPos
                               + Vector3.down * deltaYForAnim, 0.8f)
                .setEaseOutBack()
                .setDelay(0.15f);
            youGotItemPanel
                .LeanMoveLocal(youGot_startPos
                               + Vector3.down * deltaYForAnim, 0.8f)
                .setEaseOutBack()
                .setDelay(0.17f);

            sky_1
                .LeanMoveLocal(sky_1_startPos, 0.4f)
                .setEaseInBack()
                .setDelay(timeForStay);
            sky_2
                .LeanMoveLocal(sky_2_startPos, 0.4f)
                .setEaseInBack()
                .setDelay(timeForStay);
            itemPanel
                .LeanMoveLocal(itemPanel_startPos, 0.4f)
                .setEaseInBack()
                .setDelay(timeForStay);
            youGotItemPanel
                .LeanMoveLocal(youGot_startPos, 0.4f)
                .setEaseInBack()
                .setDelay(timeForStay)
                .setOnComplete(SetAnimPlayed);
        }

        public void SetAnimPlayed()
        {
            _isAnimPlay = false;
            OnAnimEndPlay?.Invoke();
        }

        public void SetAction(Action action)
        {
            if (OnAnimEndPlay != null) return;
            OnAnimEndPlay = action;
        }
        
        public void ClearAction()
        {
            OnAnimEndPlay = null;
        }
        
        public void SetItem(string title)
        {
            if (_isAnimPlay) return;
            
            itemTitlePlace.text = title;
            PlayAnim();
        }

        public void Clear()
        {
            itemTitlePlace.text = string.Empty;
        }
    }
}