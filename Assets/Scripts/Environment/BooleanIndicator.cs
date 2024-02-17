using AIAD.Exceptions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace AIAD
{
    public sealed class BooleanIndicator : MonoBehaviour
    {
        public event Action DeactivationEvent = delegate { };

        [SerializeField] private Material EmergencyMaterial;
        [SerializeField] private GameObject ActivatedLight;
        [SerializeField] private MeshRenderer Mesh;
        [SerializeField] private int MaterialIndex;
        [SerializeField] private MonoBehaviour OwnerComponent;
        [SerializeField] private MonoBehaviour PanelComponent;

        [SerializeField] private IBinaryActivStateObj Owner;
        [SerializeField] private IBinaryActivStateObj Panel;

        public bool IsActive_ { get; private set; } = false;

        private void Awake()
        {
            if (OwnerComponent != null)
            {
                string ExcSrc = "EmergencyIndicator.Awake()";

                Owner = OwnerComponent as IBinaryActivStateObj;
                Panel = PanelComponent as IBinaryActivStateObj;

                if (Owner == null)
                    throw new AIADException("Missing Owner.", ExcSrc);
                if (Panel == null)
                    throw new AIADException("Missing Panel.", ExcSrc);

                if (EmergencyMaterial == null)
                    throw new AIADException("Missing EmergencyMaterial.", ExcSrc);
                if (ActivatedLight == null)
                    throw new AIADException("Missing ActivatedLight.", ExcSrc);
                if (Mesh == null)
                    throw new AIADException("Missing MeshRenderer.", ExcSrc);

                Owner.ActivationEvent += ActivationAction;
                Owner.DeactivationEvent += DeactivationAction;
                Panel.DeactivationEvent += PanelDeactivation;
            }
        }


        private void SetMonitorMaterial(Material mat)
        {
            Material[] newMats = new Material[Mesh.materials.Length];
            Mesh.materials.CopyTo(newMats, 0);
            newMats[MaterialIndex] = mat;
            Mesh.materials = newMats;
        }
        private void ActivateIndicator()
        {
            Material oldMat = Mesh.materials[MaterialIndex];
            void ResetMaterialAction()
            {
                SetMonitorMaterial(oldMat);
                DeactivationEvent -= ResetMaterialAction;
            }
            DeactivationEvent += ResetMaterialAction;
            SetMonitorMaterial(EmergencyMaterial);
            ActivatedLight.SetActive(true);
        }
        private void DelayedActivation()
        {
            ActivateIndicator();
            Panel.ActivationEvent -= DelayedActivation;
        }
        private void ActivationAction()
        {
            if (!IsActive_)
            {
                if (Panel.IsActive_)
                {
                    ActivateIndicator();
                }
                else
                {
                    Panel.ActivationEvent += DelayedActivation;
                }
                IsActive_ = true;
            }
        }

        private void PanelDeactivation()
        {
            if (IsActive_)
            {
                DeactivateIndicator();
                Panel.ActivationEvent += DelayedActivation;
            }
        }
        private void DeactivateIndicator()
        {
            ActivatedLight.SetActive(false);
            if (Panel.IsActive_)
            {
                Panel.ActivationEvent -= DelayedActivation;
            }
            DeactivationEvent();
        }
        private void DeactivationAction()
        {
            if (IsActive_)
            {
                if (!Panel.IsActive_)
                {
                    Panel.ActivationEvent -= DelayedActivation;
                }
                DeactivateIndicator();
                IsActive_ = false;
            }
        }
    }
}
