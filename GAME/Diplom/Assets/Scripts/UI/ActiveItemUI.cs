using System;
using Interfaces.ReadOnly;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class ActiveItemUI : MonoBehaviour
    {
        [SerializeField] private RectTransform panel;
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image icon;

        private void Start()
        {
            SetInactive();
        }

        public void SetActive(Sprite icon, IReadOnlySlotData slot)
        {
            name.text = slot.GetItemName();
            description.text = slot.GetItemDescription();
            this.icon.sprite = icon;
            gameObject.SetActive(true);
        }

        public void SetInactive()
        {
            gameObject.SetActive(false);
            name.text = "";
            description.text = "";
            icon.sprite = null;
        }
    }
}