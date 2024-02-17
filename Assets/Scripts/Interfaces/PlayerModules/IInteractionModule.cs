using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public interface IInteractionModule
    {
        public event Action<IInteractiveObject> InteractiveObjHasBeenFoundEvent;
        public event Action InteractiveObjHasBeenLostEvent;
        public event Action<IInteractiveObject> InteractionEvent;
        public void Interact();
    }
}
