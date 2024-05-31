using Data;
using Enums;

namespace InteractableObjects.Characters
{
    public class WaitState : BaseCharState
    {
        public InformStatus _neededStatus;
        public WaitState(CharacterFSM fsm) : base(fsm) { }

        public override void OnStartState()
        {
            base.OnStartState();
            
            _informerInitializer.RegistrResponcer(_informerId, this);
        }

        public override void Set(CharStateData stateData)
        {
            base.Set(stateData);

            _neededStatus = stateData.informStatuses[0];
            _informerId = stateData.informerIdForObject;
            _informerInitializer.RegistrResponcer(_informerId, this);
        }

        public override void Responce(
            string whoInformer,
            InformStatus status = InformStatus.End)
        {
            if (status == _neededStatus)
            {
                _informerInitializer.UnregistrResponcer(_informerId, this);
                _fsm.NextState();
            }
        }
    }
}