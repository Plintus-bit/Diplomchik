namespace Interfaces
{
    public interface IInteractable
    {
        public bool Interact(string who = null);
        public void SetActive(bool isActive);
        public void SetSelected(bool isSelect);
        public bool IsInteractable { get; }
        
        public float XPos { get; }
    }
}