namespace Interfaces
{
    public interface IInteractManager
    {
        public bool Interact(string who);
        public void RegistrObj(IInteractable obj);
        public void UnregistrObj(IInteractable obj, bool exitFromNearBy = false);
        public void ChangeSelected(bool isRightDirect);
    }
}