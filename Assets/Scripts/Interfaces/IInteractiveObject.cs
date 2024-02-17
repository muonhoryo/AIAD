using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public interface IInteractiveObject
    {
        public event Action InteractionEvent;
        public event Action TurnOffInteractionEvent;
        public event Action TurnOnInteractionEvent;
        public bool CanInteract_ { get; }
        public void Interact();
        public void TurnOffInteraction();
        public void TurnOnInteraction();
    }
}
