using AIAD.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class MovingModuleSelector : MonoBehaviour,IMovingModule,ILockableModule
    {
        public event Action<Vector3> StartMovingEvent = delegate { };
        public event Action<Vector3> ChangeMovingDirectionEvent = delegate { };
        public event Action StopMovingEvent = delegate { };
        public event Action<IMovingModule> ChangeModuleEvent = delegate { };

        [SerializeField] private MonoBehaviour[] MovingModulesComponents;

        private IMovingModule[] MovingModules;
        private IMovingModule CurrentModule;
        private bool IsWorking = true;

        private void Awake()
        {
            if (MovingModulesComponents != null && MovingModulesComponents.Length > 0)
            {
                MovingModules = new IMovingModule[MovingModulesComponents.Length];
                for (int i = 0; i < MovingModulesComponents.Length; i++)
                {
                    MovingModules[i] = MovingModulesComponents[i] as IMovingModule;
                }
            }
            SelectModule(0);

            Registry.PlayerMovingModuleSelector = this;
        }

        private void StartMovingAction(Vector3 i)
        {
            StartMovingEvent(i);
        }
        private void ChangeMovingDirectionAction(Vector3 i)
        {
            ChangeMovingDirectionEvent(i);
        }
        private void StopMovingAction()
        {
            StopMovingEvent();
        }
        public void SelectModule(int index)
        {
            if (index >= MovingModules.Length)
                throw new AIADException("Index out of range of MovingModules[].", "MovingModuleSelector.SelectModule()");

            if (CurrentModule != null)
            {
                CurrentModule.StartMovingEvent -= StartMovingAction;
                CurrentModule.ChangeMovingDirectionEvent -= ChangeMovingDirectionAction;
                CurrentModule.StopMovingEvent-= StopMovingAction;
                CurrentModule.StopMoving();
            }
            CurrentModule = MovingModules[index];

            if (CurrentModule != null)
            {
                CurrentModule.StartMovingEvent += StartMovingAction;
                CurrentModule.ChangeMovingDirectionEvent += ChangeMovingDirectionAction;
                CurrentModule.StopMovingEvent += StopMovingAction;
            }
            ChangeModuleEvent(CurrentModule);
        }

        public bool IsMoving_ => CurrentModule != null ? CurrentModule.IsMoving_ : false;
        public Vector3 MovingDirection_ => CurrentModule != null ? CurrentModule.MovingDirection_ : Vector3.zero; 
        public Vector3 CurrentMovableObjectPosition_ => CurrentModule!=null?CurrentModule.CurrentMovableObjectPosition_ : Vector3.zero;

        void IMovingModule.StartMovingAction(Vector2 direction)
        {
            if (CurrentModule != null)
            {
                CurrentModule.SetMovingDirection(direction);
            }
        }
        void IMovingModule.SetMovingDirectionAction(Vector2 direction)
        {
            if (CurrentModule != null)
            {
                CurrentModule.SetMovingDirection(direction);
            }
        }
        void IMovingModule.StopMovingAction()
        {
            if (CurrentModule != null)
            {
                CurrentModule.StopMoving();
            }
        }

        bool ILockableModule.IsLocked_ => IsWorking;
        void ILockableModule.Lock() => IsWorking = false;
        void ILockableModule.Unlock() => IsWorking = true;
    }
}
