using AIAD.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class InteractionByViewModule : MonoBehaviour,IInteractionModule,ILockableModule
    {
        public event Action<IInteractiveObject> InteractiveObjHasBeenFoundEvent=delegate { };
        public event Action InteractiveObjHasBeenLostEvent = delegate { };
        public event Action<IInteractiveObject> InteractionEvent = delegate { };

        [SerializeField] private Transform SphereCastDataOrigin;
        [SerializeField][Range(0,10)] private float SphereCastRadius;
        [SerializeField][Range(0, 10)] private float SphereCastDistance;
        [SerializeField] private int LayerMask;

        private IInteractiveObject InteractiveObject;

        private void Awake()
        {
            string ExcSrc = "InteractionByViewModule.Awake()";

            if (SphereCastDataOrigin == null)
                throw new AIADException("SphereCastDataOrigin cannot be null.", ExcSrc);
            if (SphereCastRadius <= 0)
                throw new AIADException("SphereCastRadius must be greater than zero", ExcSrc);
            if (SphereCastDistance <= 0)
                throw new AIADException("SphereCastDistance must be greater than zero", ExcSrc);
            if (LayerMask < 0)
                throw new AIADException("LayerMask must be equal or greater than zero", ExcSrc);
        }
        private void Update()
        {
            IInteractiveObject obj=null;
            RaycastHit[] hitsInfo=Physics.SphereCastAll
                (origin: SphereCastDataOrigin.position,
                radius: SphereCastRadius,
                direction: SphereCastDataOrigin.eulerAngles.DirectionFromAngle3D(),
                maxDistance: SphereCastDistance,
                layerMask: LayerMask,
                queryTriggerInteraction: QueryTriggerInteraction.Collide);
            foreach(var hit in hitsInfo)
            {
                obj = hit.collider.gameObject.GetComponentInParent<IInteractiveObject>();
                if (obj != null&&obj.CanInteract_)
                    break;
            }
            if (obj != null)
            {
                InteractiveObject = obj;
                InteractiveObjHasBeenFoundEvent(InteractiveObject);
            }
            else if (InteractiveObject != null)
            {
                InteractiveObject = null;
                InteractiveObjHasBeenLostEvent();
            }
        }

        public void Interact()
        {
            if (InteractiveObject != null)
            {
                InteractiveObject.Interact();
                InteractionEvent(InteractiveObject);
            }
        }

        void ILockableModule.Lock()
        {
            enabled = false;
            InteractiveObject = null;
            InteractiveObjHasBeenLostEvent();
        }
        void ILockableModule.Unlock()=> enabled = true;

        bool ILockableModule.IsLocked_ => !enabled;
    }
}
