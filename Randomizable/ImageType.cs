using System;
using Interfaces;
using UnityEngine.UI;

[Serializable]
public class ImageType : IChance
{
    public int chance;
    public Image image;
    public int Chance => chance;

    public void Hide()
    {
        image.enabled = false;
    }

    public void Show()
    {
        image.enabled = true;
    }
}