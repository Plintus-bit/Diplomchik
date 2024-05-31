using Brainteasers;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BedTableBrainteaserItemUI : BaseBrainTeaserItemUI<bool>
    {
        private BedTableBrainteaserItem _item;
        public Image image;

        public override void Init(BaseBrainTeaser brainTeaser, bool value, int index)
        {
            if (image == null) image = GetComponent<Image>();
            
            if (_item == null)
            {
                _item = new BedTableBrainteaserItem(brainTeaser, value, index);
            }
            else
            {
                _item.Init(brainTeaser, value, index);
            }
        }

        public override bool Change(
            BrainTeaserItemChangeCondition condition
            = BrainTeaserItemChangeCondition.Next)
        {
            bool newValue = _item.Change(condition);
            if (newValue)
            {
                image.color = Color.green;
            }
            else
            {
                image.color = Color.white;
            }

            return newValue;
        }
        
        public void OnUIAction()
        {
            _item.OnUIAction();
        }
    }
}