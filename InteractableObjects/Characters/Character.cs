using Data;
using Enums;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterData _data;
    [SerializeField] private Inventory.Inventory _inventory;
    
    public Inventory.Inventory CharacterInventory => _inventory;
    
    private void Start()
    {
        _data.hasInventory = _inventory != null;
    }

    public CharacterTypes GetCharType()
    {
        return _data.type;
    }

    public string GetName()
    {
        return _data.name;
    }

    public bool HasInventory()
    {
        return _data.hasInventory;
    }
}