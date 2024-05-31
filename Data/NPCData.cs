using System.Collections.Generic;
using Interfaces.ReadOnly;
using UnityEngine;

namespace Data
{
    public class NPCData : IReadOnlyNPCData
    {
        public Color color;
        public Color nameColor;
        public string name;

        public static Color DefaultNameColor = new Color(236 / 256f, 209 / 256f, 86 / 256f);
        public static Color DefaultColor = Color.grey;
        
        public Dictionary<string, Sprite> charImages;
        public Sprite defaultImage;

        public NPCData(string name, Color color, Color nameColor)
        {
            this.name = name;
            
            this.color = GetColor(color, DefaultColor);
            this.nameColor = GetColor(nameColor, DefaultNameColor);

            charImages = new Dictionary<string, Sprite>();
            defaultImage = Resources.Load<Sprite>("Characters/NPC_default");
        }

        private Color GetColor(Color whatColor, Color defaultColor)
        {
            if (whatColor == Color.clear)
            {
                return defaultColor;
            }
            return whatColor;
        }
        
        public string Name => name;
        public Color Color => color;
        public Color NameColor => nameColor;
        public Sprite GetImage(string state)
        {
            if (charImages.ContainsKey(state)) return charImages[state];
            return defaultImage;
        }

        public void TryAddImageState(string state)
        {
            if (charImages.ContainsKey(state)) return;
            Sprite image = Resources.Load<Sprite>(
                "Characters/" + name + "_" + state);
            if (image == null) return;
            charImages.Add(state, image);
        }
    }
}