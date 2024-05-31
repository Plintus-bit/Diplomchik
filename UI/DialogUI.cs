using Interfaces.ReadOnly;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DialogUI : MonoBehaviour
    {
        [SerializeField] private UITextWriter _textWriter;
        
        public Image dialogBackground;
        public Image character;
        public TMP_Text author;
        public TMP_Text text;

        public void Change(IReadOnlyDialogData data)
        {
            IReadOnlyNPCData npc = data.Author;
            author.text = npc.Name;
            character.sprite = npc.GetImage(data.State);
            dialogBackground.color = npc.Color;
            author.color = npc.NameColor;
            _textWriter.AddToWriter(text, data.Text, 0.018f);
        }

        public void Clear()
        {
            dialogBackground.color = Color.white;
            author.text = "";
            text.text = "";
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}