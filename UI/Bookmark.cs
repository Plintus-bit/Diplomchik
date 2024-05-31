using System;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Bookmark : MonoBehaviour
    {
        public static BookmarksManager parent;
        public Image background;
        public TMP_Text name;

        public bool isActive;

        public void Set(string name, Sprite background)
        {
            this.background.sprite = background;
            this.name.text = name;
            isActive = false;
        }

        public void OnClick()
        {
            parent.OnBookmarkClick(name.text);
        }

        public void Clear()
        {
            name.text = "";
        }
        
        public void Open()
        {
            if (isActive) return;
            transform.LeanMoveLocal(
                transform.localPosition
                + new Vector3(40, 0, 0),
                0.1f);
            isActive = true;
        }

        public void Close()
        {
            if (!isActive) return;
            transform.LeanMoveLocal(
                transform.localPosition
                + new Vector3(-40, 0, 0),
                0.1f);
            isActive = false;
        }
    }
}