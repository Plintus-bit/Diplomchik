using System;
using System.Collections.Generic;
using Enums;
using Interfaces.ReadOnly;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class AbilityPanelUI : MonoBehaviour
{
    [SerializeField] private RectTransform _parent;
    [SerializeField] private RectTransform _itemsPanel;
    [SerializeField] private Image _arrowPlace;
    [SerializeField] private List<AbilityUI> _abils;

    public Sprite leftArrow;
    public Sprite rightArrow;
    public float panelWidth;

    public bool isOpened;

    private void Start()
    {
        panelWidth = _itemsPanel.rect.width;
        isOpened = false;
    }

    public void TryOpenOrClose()
    {
        if (isOpened) Close();
        else Open();
    }

    public void Open()
    {
        _arrowPlace.sprite = leftArrow;
        
        BasicAnimations.MoveLocalTransform(
            _parent, new Vector3(panelWidth, 0, 0),
            0.15f, BasicAnimConditions.Positive);
        
        isOpened = !isOpened;
    }

    public void Close()
    {
        _arrowPlace.sprite = rightArrow;

        BasicAnimations.MoveLocalTransform(
            _parent, new Vector3(panelWidth, 0, 0),
            0.15f, BasicAnimConditions.Negative);
        
        isOpened = !isOpened;
    }

    public void AddAbility(IReadOnlyAbility abil)
    {
        foreach (var ability in _abils)
        {
            if (!ability.HasItem())
            {
                ability.SetAbility(abil);
                return;
            }
        }
    }
}
