using System;
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD
{
    public sealed class ControlPanel : MonoBehaviour,IBinaryActivStateObj
    {
        public event Action ActivationEvent = delegate { };
        public event Action DeactivationEvent = delegate { };

        [SerializeField] private MonoBehaviour OwnerComponent;
        [SerializeField] private EmergencyState EmergencyState;

        private IBinaryActivStateObj Owner;

        public bool IsActive_ { get; private set; } = false;

        private void Awake()
        {
            string ExcSrc = "ControlPanel.Awake()";

            Owner = OwnerComponent as IBinaryActivStateObj;
            if (Owner == null)
                throw new AIADException("Missing Owner.", ExcSrc);
            if (EmergencyState == null)
                throw new AIADException("Missing EmergencyState", ExcSrc);

            Owner.ActivationEvent += ActivatePanel;
            Owner.DeactivationEvent += DeactivatePanel;
            EmergencyState.OnEmergencyEvent += DeactivatePanel;
        }
        public void ActivatePanel()
        {
            if (!IsActive_)
            {
                IsActive_ = true;
                ActivationEvent();
            }
        }
        public void DeactivatePanel()
        {
            if (IsActive_)
            {
                IsActive_ = false;
                DeactivationEvent();
            }
        }
    }
}
