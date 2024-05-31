using System;
using System.Collections.Generic;
using Interfaces;
using Managers;

namespace Data
{
    [Serializable]
    public class Locker
    {
        public string lockerId;
        public List<ILockable> whatLock;

        public static LockerInitializer _LockerInitializer;

        // FLAGS
        public bool lockOnAdd = true; 
        public bool unlockOnRemove = true;
        
        public void Init()
        {
            whatLock = new List<ILockable>();
            
            if (string.IsNullOrEmpty(lockerId)) return;
            _LockerInitializer ??= LockerInitializer.GetInstance();
            _LockerInitializer.AddLocker(this);
        }

        public void Unlock(bool isForever = false)
        {
            if (whatLock == null) return;

            foreach (var item in whatLock)
            {
                item.Unlock();
            }

            if (isForever)
            {
                whatLock.Clear();
            }
        }

        public void Lock()
        {
            if (whatLock == null) return;
            foreach (var item in whatLock)
            {
                item.Lock();
            }
        }

        public void AddWhatToLock(ILockable what)
        {
            if (whatLock.Contains(what)) return;
            if (lockOnAdd) what.Lock();
            whatLock.Add(what);
        }

        public void RemoveWhatToLock(ILockable what)
        {
            if (!whatLock.Contains(what)) return;
            if (unlockOnRemove) what.Unlock();
            whatLock.Remove(what);
        }

        public void Clear()
        {
            whatLock.Clear();
            _LockerInitializer.RemoveLocker(lockerId);
        }
    }
}