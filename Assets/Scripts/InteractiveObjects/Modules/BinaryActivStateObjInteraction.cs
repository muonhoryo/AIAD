using System;
using AIAD.Exceptions;
using AIAD.SL;
using UnityEngine;

namespace AIAD
{
    public sealed class BinaryActivStateObjInteraction : SLInteractiveObject
    {

        [SerializeField] private BinaryActivityStateObject Owner;

        protected override void AwakeAction_BeforeSL()
        {
            if (Owner == null)
                throw new AIADException("Missing Tumbler script.", "BinaryActivStateObjInteraction.AwakeAction_BeforeSL()");

            InteractionEvent += InteractOwner;
        }
        public void InteractOwner()
        {
            if (Owner.IsActive_)
            {
                Owner.TurnOff();
            }
            else
            {
                Owner.TurnOn();
            }
        }
    }
}
