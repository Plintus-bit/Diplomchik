using Enums;
using Interfaces.ReadOnly;
using UnityEngine;

namespace Data
{
    public class Author : IReadOnlyAuthor
    {
        public ProofType author;
        public string name;
        public string description;
        public Sprite image;

        public ProofType AuthorType => author;
        public string Name => name;
        public string Description => description;
        public Sprite AuthorImage => image;
        
        public Author(ProofType author, string name, string description)
        {
            this.author = author;
            this.name = name;
            this.description = description;
            SetImage();
        }

        private void SetImage()
        {
            image = Resources.Load<Sprite>("Characters/" + name);
        }
    }
}