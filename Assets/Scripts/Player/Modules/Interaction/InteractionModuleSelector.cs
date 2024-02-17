using AIAD.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM 
{
    public class InteractionModuleSelector : MonoBehaviour,IInteractionModule,ILockableModule
    {
        public event Action<IInteractiveObject> InteractiveObjHasBeenFoundEvent = delegate { };
        public event Action InteractiveObjHasBeenLostEvent = delegate { };
        public event Action<IInteractiveObject> InteractionEvent = delegate { };
        public event Action<IInteractionModule> ChangeModuleEvent = delegate { };

        [SerializeField] private MonoBehaviour[] InteractionModulesComponents;

        private IInteractionModule[] InteractionModules;
        private IInteractionModule CurrentModule;
        private bool CanInteract = true;

        private void Awake()
        {
            if (InteractionModulesComponents != null && InteractionModulesComponents.Length > 0)
            {
                InteractionModules = new IInteractionModule[InteractionModulesComponents.Length];
                for (int i = 0; i < InteractionModulesComponents.Length; i++)
                {
                    InteractionModules[i] = InteractionModulesComponents[i] as IInteractionModule;
                }
            }
            SelectModule(0);

            Registry.PlayerInteractionModuleSelector = this;
        }

        private void InteractiveObjFoundAction(IInteractiveObject obj) =>
            InteractiveObjHasBeenFoundEvent(obj);
        private void InteractiveObjLostAction() =>
            InteractiveObjHasBeenLostEvent();
        private void InteractionAction(IInteractiveObject obj) =>
            InteractionEvent(obj);
        public void SelectModule(int index)
        {
            if (index >= InteractionModules.Length)
                throw new AIADException("Index out of range of InteractionModules[].", "InteractionModuleSelector.SelectModule()");

            if (CurrentModule != null)
            {
                CurrentModule.InteractiveObjHasBeenFoundEvent -= InteractiveObjFoundAction;
                CurrentModule.InteractiveObjHasBeenLostEvent -= InteractiveObjLostAction;
                CurrentModule.InteractionEvent -= InteractionAction;
            }
            CurrentModule = InteractionModules[index];

            if (CurrentModule != null)
            {
                CurrentModule.InteractiveObjHasBeenFoundEvent += InteractiveObjFoundAction;
                CurrentModule.InteractiveObjHasBeenLostEvent += InteractiveObjLostAction;
                CurrentModule.InteractionEvent += InteractionAction;
            }
            ChangeModuleEvent(CurrentModule);
        }
        

        void IInteractionModule.Interact()
        {
            if(CanInteract)
                CurrentModule?.Interact();
        }
        bool ILockableModule.IsLocked_ => CanInteract;
        void ILockableModule.Lock() 
        {
            CanInteract = false;
        }
        void ILockableModule.Unlock() => CanInteract = true;
    }
}

