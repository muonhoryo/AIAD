using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class ControlLockModule : MonoBehaviour,IControlLockModule
    {
        public event Action LockControlEvent = delegate { };
        public event Action UnlockControlEvent = delegate { };

        [SerializeField] private MonoBehaviour[] TurnedOffModulesComponents;

        private ILockableModule[] TurnedOffModules;

        public bool IsLocked_ { get; private set; }

        private void Awake()
        {
            List<ILockableModule> modules = new List<ILockableModule>(TurnedOffModulesComponents.Length);
            foreach(var module in TurnedOffModulesComponents)
            {
                ILockableModule parsedMod= module as ILockableModule;
                if(parsedMod!=null)
                    modules.Add(parsedMod);
            }
            TurnedOffModules=modules.ToArray();
            TurnedOffModulesComponents = null;

            Registry.ControlLocker = this;
        }

        public void LockControl()
        {
            foreach(var mod in TurnedOffModules)
            {
                mod.Lock();
            }
            LockControlEvent();
        }
        public void UnlockControl()
        {
            foreach(var mod in TurnedOffModules)
            {
                mod.Unlock();
            }
            UnlockControlEvent();
        }
    }
}
