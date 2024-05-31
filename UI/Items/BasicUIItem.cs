using System;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Items
{
    public class BasicUIItem : UIInputListener
    {
        [SerializeField] private BasicUIWindow _parentWindow;
        [SerializeField] private string iconName;
        [SerializeField] private Image icon;
        
        protected static ICharacterService _charService;

        protected bool isSelected;

        protected override void Initialize()
        {
            _charService ??= FindObjectOfType<CharacterService>();
            _parentWindow = GetComponentInParent<BasicUIWindow>();
            SetIconName();
            SetIcon();
            isSelected = false;
            SetIconState(isSelected);
        }

        public virtual void SetIconName()
        {
            if (iconName != String.Empty) return;
            iconName = "ExploreIcon";
        }
        
        public virtual void Inspect() {}
        
        private void SetIcon()
        {
            if (icon == null)
            {
                throw new Exception(
                    "Не поставила Image иконки активности в UIItem");
            }
            icon.sprite = Resources.Load<Sprite>("UI/Active" + iconName);
        }

        public virtual void OnStartInspect()
        {
            _parentWindow.TurnOff();
            _parentWindow.TurnOffAllChildren();
            SetIconState(false);
        }
        
        public virtual void OnEndInspect()
        {
            _parentWindow.TurnOn();
            _parentWindow.TurnOnAllChildren();
        }

        public void OnSelect()
        {
            isSelected = true;
            SetIconState(true);
        }

        public void OnDeselect()
        {
            isSelected = false;
            SetIconState(false);
        }

        public void SetIconState(bool isActive)
        {
            // if (!isSelected && isActive) return;
            
            icon.enabled = isActive;
        }
    }
}