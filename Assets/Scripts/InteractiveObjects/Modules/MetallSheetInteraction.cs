using AIAD.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class MetallSheetInteraction : SLInteractiveObject
    {
        [SerializeField] private BoxCollider Collider;
        [SerializeField] private Vector3 BoxCastOffset;
        [SerializeField] private int LayerMask;
        public override bool CanInteract_ => !IsTherePlayer()&&CanInteract;

        private bool IsTherePlayer()
        {
            Collider[] colls = Physics.OverlapBox(
                center: Collider.transform.position + Collider.center + BoxCastOffset,
                halfExtents: Collider.size*0.5f,
                orientation: Quaternion.Euler(Collider.transform.eulerAngles),
                layerMask: LayerMask,
                queryTriggerInteraction: QueryTriggerInteraction.Collide);
            foreach(var col in colls)
            {
                PlayerController player;
                player=col.GetComponentInParent<PlayerController>();
                if (player != null)
                    return true;
            }
            return false;
        }
    }
}
