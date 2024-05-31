using Enums;

namespace Interfaces
{
    public interface IResponcer
    {
        public void Responce(string whoInformer,
            InformStatus status = InformStatus.End);
    }
}