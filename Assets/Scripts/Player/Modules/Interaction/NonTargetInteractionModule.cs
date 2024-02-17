using AIAD.Player.COM;
using AIAD.SL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class NonTargetInteractionModule : MonoBehaviour,IInteractionModule
    {
        public event Action<IInteractiveObject> InteractiveObjHasBeenFoundEvent { add { } remove { } }
        public event Action InteractiveObjHasBeenLostEvent { add { } remove { } }
        public event Action<IInteractiveObject> InteractionEvent = delegate { };

        [SerializeField] private string SLScriptsID;

        private void Awake()
        {
            if (!string.IsNullOrEmpty(SLScriptsID))
                InteractionEvent += (i) => SLScriptsID.RunSLScripts();
        }

        public void Interact()
        {
            InteractionEvent(null);
        }
    }
}
