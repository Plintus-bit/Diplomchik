using System;
using Data;
using Enums;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterData data;
    
    private void Start()
    {
        var inventory = GetComponentInChildren<Inventory.Inventory>();
        data.hasInventory = inventory != null;
    }

    public CharacterTypes GetCharType()
    {
        return data.type;
    }

    public string GetName()
    {
        return data.name;
    }

    public bool HasInventory()
    {
        return data.hasInventory;
    }
}