using System;
using Interfaces;
using UnityEngine;

[Serializable]
public class SpriteType : IChance
{
    public int chance;
    public Sprite sprite;

    public int Chance => chance;
}