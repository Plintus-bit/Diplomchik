using System.Collections.Generic;
using Interfaces.ReadOnly;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProofPlaceUI : MonoBehaviour
    {
        [SerializeField] private List<SpriteType> _tapeTypes;
        [SerializeField] private RectTransform _rect;
        [SerializeField] private RectTransform _descriptionRect;
        private Vector2 _basicSize;
        private Vector2 _basicDescriptionSize;

        public CanvasGroup place;
        public TMP_Text title;
        public TMP_Text description;
        public Image tapePlace;
        public TMP_Text readyValue;

        private void Awake()
        {
            place = GetComponent<CanvasGroup>();
            _rect = GetComponent<RectTransform>();
            _basicSize = _rect.rect.size;
            // _descriptionRect = description.rectTransform;
            // _basicDescriptionSize = _descriptionRect.rect.size;
        }

        public void SetTape()
        {
            tapePlace.sprite = Randomizer<SpriteType>
                .Randomize(_tapeTypes).sprite;
        }

        public void Clear()
        {
            title.text = "";
            description.text = "";
            readyValue.text = "";
            readyValue.enabled = false;
            _rect.sizeDelta = _basicSize;
            // _descriptionRect.sizeDelta = _basicDescriptionSize;
        }

        public void Set(IReadOnlyProofData data)
        {
            title.text = data.Title;
            List<string> descriptions = data.GetDescriptions();

            foreach (var item in descriptions)
            {
                description.text += item + "\n";
            }
            
            ChangeSizeAndMove(
                _rect, transform, _basicSize, description.text.Length,
                descriptions.Count - 1, 0.3f);

            if (data.MaxAmount == 1)
            {
                readyValue.enabled = false;
            }
            else
            {
                readyValue.enabled = true;
                readyValue.text = $"{data.Amount}/{data.MaxAmount}";
            }
            SetTape();
        }
        
        public void ChangeSizeAndMove(RectTransform what, Transform whatTransform,
            Vector2 basic, int textLength, int howMuch,
            float multiplier, float posMultiplier = 0.5f, bool needMove = true)
        {
            // float value = basic.y * multiplier * howMuch;
            float value = textLength / 15f * basic.y / 2.9f * multiplier;
            what.sizeDelta = new Vector2(basic.x, basic.y + value);
            if (needMove)
            {
                whatTransform.transform.localPosition
                    -= new Vector3(0, value * posMultiplier, 0);
            }
        }

        public float SizeAndBasicSizeDelta()
        {
            return _rect.rect.size.y - _basicSize.y;
        }
        
        public bool IsEmpty()
        {
            return description.text == "";
        }

        public void Hide()
        {
            place.alpha = 0;
        }

        public void Show()
        {
            place.alpha = 1;
        }
    }
}