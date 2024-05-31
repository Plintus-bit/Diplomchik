using System.Collections.Generic;

namespace Managers
{
    public class BaseLoader<T>
    {
        public string path;
        
        protected virtual void ReadDataFromFile()
        {
            // do nothing
        }

        public virtual bool HasItem(string itemId)
        {
            return false;
        }

        public virtual T GetDataById(string id)
        {
            T item = default;
            return item;
        }
    }
}