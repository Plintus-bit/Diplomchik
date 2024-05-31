
namespace InteractableObjects
{
    public class DialogObject : BasicDialogObject
    {
        public override bool Interact(string who)
        {
            StartDialog(who);
            return IsInteractable;
        }
    }
}