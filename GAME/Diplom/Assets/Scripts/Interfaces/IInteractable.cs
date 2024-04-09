namespace Interfaces
{
    public interface IInteractable
    {
        public bool Interact(string who);
        public void SetActive(bool isActive);
        public void SetSelected(bool isSelect);
        public bool IsInteractable { get; }
    }
}