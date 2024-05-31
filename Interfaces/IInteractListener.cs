namespace Interfaces
{
    public interface IInteractListener
    {
        public void ReactOnNotify(IInteractable obj, bool isInteractable);
        public void RegistrListener();
        public void UnregistrListener();
    }
}