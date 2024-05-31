using Enums;

namespace Interfaces.ReadOnly
{
    public interface IReadOnlyTask
    {
        public string Title { get; }
        public string Description { get;  }
        public TaskStatuses Status { get; }

    }
}