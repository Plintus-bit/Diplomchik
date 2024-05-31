using System.Collections.Generic;
using Data;
using Interfaces;

namespace Managers
{
    public class LockerInitializer
    {
        private static LockerInitializer _instance;

        private Dictionary<string, Locker> _lockers;
        private Dictionary<string, List<ILockable>> _needAddToLocker;

        public LockerInitializer()
        {
            if (_instance != null) return;
            _lockers = new Dictionary<string, Locker>();
            _needAddToLocker = new Dictionary<string, List<ILockable>>();
            _instance = this;
        }
        
        private void CheckAndAdd(string lockerId)
        {
            if (!_needAddToLocker.ContainsKey(lockerId)) return;
            List<ILockable> needAdd = _needAddToLocker[lockerId];
            Locker locker = _lockers[lockerId];
            foreach (var lockable in needAdd)
            {
                locker.AddWhatToLock(lockable);
            }
            _needAddToLocker[lockerId].Clear();
            _needAddToLocker.Remove(lockerId);
        }

        public static LockerInitializer GetInstance()
        {
            _instance ??= new LockerInitializer();
            return _instance;
        }

        public void AddLocker(Locker locker)
        {
            if (string.IsNullOrEmpty(locker.lockerId)) return;
            if (_lockers.ContainsKey(locker.lockerId)) return;
            _lockers.Add(locker.lockerId, locker);
            CheckAndAdd(locker.lockerId);
        }

        public void RemoveLocker(string id)
        {
            if (_lockers.ContainsKey(id)) _lockers.Remove(id);
        }

        public void AddItemToLocker(string lockerId, ILockable lockableObject)
        {
            if (string.IsNullOrEmpty(lockerId)) return;
            
            if (!_lockers.ContainsKey(lockerId))
            {
                if (!_needAddToLocker.ContainsKey(lockerId))
                {
                    _needAddToLocker.Add(lockerId, new List<ILockable>());
                }
                _needAddToLocker[lockerId].Add(lockableObject);
                return;
            }
            _lockers[lockerId].AddWhatToLock(lockableObject);
        }
        
        public void RemoveItemFromLocker(string lockerId, ILockable lockableObject)
        {
            if (!_lockers.ContainsKey(lockerId)) return;
            _lockers[lockerId].RemoveWhatToLock(lockableObject);
        }
    }
}