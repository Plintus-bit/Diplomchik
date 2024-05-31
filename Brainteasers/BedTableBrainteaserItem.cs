using Enums;

namespace Brainteasers
{
    public class BedTableBrainteaserItem : BaseBrainTeaserItem<bool>
    {
        public BedTableBrainteaserItem() {}
        
        public BedTableBrainteaserItem(
            BaseBrainTeaser brainTeaser,
            bool value, int index)
        {
            Init(brainTeaser, value, index);
        }
        
        public void Init(BaseBrainTeaser brainTeaser,
            bool value, int index)
        {
            SetBrainteaser(brainTeaser);
            SetValue(value);
            SetIndex(index);
        }
        
        public override bool Change(BrainTeaserItemChangeCondition condition
            = BrainTeaserItemChangeCondition.Next)
        {
            value = !value;
            return value;
        }
    }
}