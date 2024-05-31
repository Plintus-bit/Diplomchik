using Enums;

namespace Interfaces
{
    public interface IInformer
    {
        public void Inform(InformStatus status = InformStatus.End);
        public void Unsubscribe(IResponcer who);
        public void Subscribe(IResponcer who);
        public string InformerId { get; }
    }
}