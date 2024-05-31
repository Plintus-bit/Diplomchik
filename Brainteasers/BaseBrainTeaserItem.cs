using Enums;

namespace Brainteasers
{
    public class BaseBrainTeaserItem<T>
    {
        private BaseBrainTeaser _brainteaser;
        public int index;
        public T value;

        public void SetBrainteaser(BaseBrainTeaser brainteaser)
        {
            _brainteaser = brainteaser;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }
        
        public T SetValue(T newValue)
        {
            value = newValue;
            return value;
        }
        
        public virtual void OnUIAction()
        {
            _brainteaser.OnUIAction(index);
        }

        public virtual T Change(BrainTeaserItemChangeCondition condition
            = BrainTeaserItemChangeCondition.Next)
        {
            return value;
        }
    }
}