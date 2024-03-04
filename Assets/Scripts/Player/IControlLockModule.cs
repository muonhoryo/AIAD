using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIAD.Player.COM
{
    public interface IControlLockModule
    {
        public event Action LockControlEvent;
        public event Action UnlockControlEvent;
        public bool IsLocked_ { get; }
        public void LockControl();
        public void UnlockControl();
    }
    public interface ILockableModule
    {
        public bool IsLocked_ { get; }
        public void Lock();
        public void Unlock();
    }
}
