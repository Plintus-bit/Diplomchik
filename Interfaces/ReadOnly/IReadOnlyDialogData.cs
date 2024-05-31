using Data;

namespace Interfaces.ReadOnly
{
    public interface IReadOnlyDialogData
    {
        public NPCData Author { get; }
        public string Text { get; }
        public string State { get; }
    }
}