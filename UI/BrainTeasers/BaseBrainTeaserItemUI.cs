using Brainteasers;
using Enums;

namespace UI
{
    public abstract class BaseBrainTeaserItemUI<T> : UIInputListener
    {
        public abstract void Init(BaseBrainTeaser brainTeaser, T value, int index);
        public abstract T Change(
            BrainTeaserItemChangeCondition condition
            = BrainTeaserItemChangeCondition.Next);
    }

}