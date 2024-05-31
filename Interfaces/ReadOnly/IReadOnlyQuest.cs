using Enums;

namespace Interfaces.ReadOnly
{
    public interface IReadOnlyQuest
    {
        public TaskStatuses Status { get; }
    }
}