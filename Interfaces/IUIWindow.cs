namespace Interfaces
{
    public interface IUIWindow
    {
        public void Close(bool isForever = false);
        public void Open();
    }
}