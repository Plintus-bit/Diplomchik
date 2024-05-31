using Brainteasers;

namespace UI
{
    public class WatchUI : BaseBrainTeaserUI
    {
        private Watch _watch;

        public Watch WatchBrain
        {
            get => _watch;
            set => _watch = value;
        }
        
        public void RotateMinuteHand(bool isRightDir)
        {
            if (_watch.OnRotate(isRightDir))
            {
                OnSolveState();
            }
        }

        public void OnSolveState()
        {
            if (_watch.TryChangeState(CloseAction))
            {
                TurnOff();
                TurnOffAllChildren();
                return;
            }
            Close();
        }

        public void CloseAction()
        {
            Close();
        }
    }
}