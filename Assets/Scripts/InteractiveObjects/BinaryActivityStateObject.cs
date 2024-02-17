
using AIAD.Exceptions;
using AIAD.SL;
using System;
using UnityEngine;


namespace AIAD
{
    public sealed class BinaryActivityStateObject : MonoBehaviour,IBinaryActivStateObj
    {
        public event Action ActivationEvent = delegate { };
        public event Action DeactivationEvent = delegate { };
        [SerializeField] private string SLScriptIDs_Activation;
        [SerializeField] private string SLScriptIDs_Deactivation;

        [SerializeField] private bool IsTurnedOnStart = false;
        [SerializeField] private Animator Animator;
        [SerializeField] private string AnimBoleanName;

        public bool IsActive_ { get; private set; } = false;

        private void Awake()
        {
            if (Animator == null)
                throw new AIADException("Missing Animator.", "BinaryActivityStateObject.Awake()");

            if (!string.IsNullOrEmpty(SLScriptIDs_Activation))
                ActivationEvent += () => SLScriptIDs_Activation.RunSLScripts();
            if (!string.IsNullOrEmpty(SLScriptIDs_Deactivation))
                DeactivationEvent += () => SLScriptIDs_Deactivation.RunSLScripts();

            if (IsTurnedOnStart)
                TurnOn();
        }

        public void TurnOn()
        {
            if (!IsActive_)
            {
                Animator.SetBool(AnimBoleanName,true);
                IsActive_ = true;
                ActivationEvent();
            }
        }
        public void TurnOff()
        {
            if (IsActive_)
            {
                Animator.SetBool(AnimBoleanName,false);
                IsActive_ = false;
                DeactivationEvent();
            }
        }
    }
}
