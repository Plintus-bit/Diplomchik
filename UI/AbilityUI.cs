using Interfaces.ReadOnly;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AbilityUI : MonoBehaviour
    {
        [SerializeField] private Image _imagePlace;

        private void Start()
        {
            _imagePlace = GetComponent<Image>();
        }

        public void SetAbility(IReadOnlyAbility abil)
        {
            _imagePlace.sprite = abil.AbilImage;
        }

        public bool HasItem()
        {
            if (_imagePlace.sprite.name == "Empty") return false;
            return true;
        }
        
    }
}