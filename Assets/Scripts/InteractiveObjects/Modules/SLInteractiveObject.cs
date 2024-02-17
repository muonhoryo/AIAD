using AIAD.SL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public class SLInteractiveObject : MonoBehaviour,IInteractiveObject
    {
        public event Action InteractionEvent=delegate { };
        public event Action TurnOffInteractionEvent = delegate { };
        public event Action TurnOnInteractionEvent = delegate { };
        [SerializeField] private string SLScriptsIDs_Interaction;
        [SerializeField] private bool IsCanInteractOnStart = true;

        protected bool CanInteract;
        public virtual bool CanInteract_ { get=>CanInteract; protected set=>CanInteract=value; }

        private void Awake()
        {
            AwakeAction_BeforeSL();
            if (!string.IsNullOrEmpty(SLScriptsIDs_Interaction))
                InteractionEvent += () => SLScriptsIDs_Interaction.RunSLScripts();
            if (IsCanInteractOnStart)
                TurnOnInteraction();
            else
                TurnOffInteraction();
            AwakeAction_AfterSL();
        }

        public void Interact()
        {
            if(CanInteract_)
                InteractionEvent();
        }
        public void TurnOnInteraction()
        {
            CanInteract_ = true;
            TurnOnInteractionEvent();
        }
        public void TurnOffInteraction()
        {
            CanInteract_ = false;
            TurnOffInteractionEvent();
        }

        protected virtual void AwakeAction_BeforeSL() { }
        protected virtual void AwakeAction_AfterSL() { }
    }
}
