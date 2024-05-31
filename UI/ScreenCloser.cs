using System;
using UnityEngine;

namespace UI
{
    public class ScreenCloser : MonoBehaviour
    {
        private float _time;
        private Action _tempActionToInvoke;
        
        public CanvasGroup canvasGroup;

        private void Start()
        {
            _time = 0.3f;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void OpenScreenAnim(Action action)
        {
            canvasGroup.blocksRaycasts = true;
            _tempActionToInvoke = action;
            
            canvasGroup
                .LeanAlpha(0, _time)
                .setDelay(0.24f)
                .setOnComplete(OnEndAnim);
        }

        public void CloseScreenAnim(Action action)
        {
            canvasGroup.blocksRaycasts = true;
            _tempActionToInvoke = action;
            
            canvasGroup
                .LeanAlpha(1, _time)
                .setOnComplete(OnEndAnim);
        }

        public void OnEndAnim()
        {
            canvasGroup.blocksRaycasts = false;
            _tempActionToInvoke?.Invoke();
        }

        public void ClearAction()
        {
            _tempActionToInvoke = null;
        }
    }
}