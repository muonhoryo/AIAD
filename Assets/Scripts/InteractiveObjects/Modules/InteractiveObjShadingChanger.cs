
using AIAD.Exceptions;
using System.Collections;
using UnityEngine;

namespace AIAD
{
    public sealed class InteractiveObjShadingChanger : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour Owner;
        [SerializeField] private MeshRenderer[] OwnerMeshes;
        [SerializeField] private float LightingDelay;
        [SerializeField] private float PowerChangeSpeed;
        [SerializeField] private float MaxLightingPower;
        [SerializeField] private string LightingPowerName;

        private Coroutine CurrentChanger=null;
        private float CurrentPower = 0;

        private void OnDeactivationInteraction()
        {
            if(CurrentChanger!=null)
                StopCoroutine(CurrentChanger);
            SetLightingPower(0);
            CurrentChanger = null;
        }
        private void SetLightingPower(float power)
        {
            foreach(var m in OwnerMeshes)
            {
                Material[] mats = m.materials;
                foreach(var mat in mats)
                {
                    mat.SetFloat(LightingPowerName, power);
                }
            }
            CurrentPower = power;
        }
        private IEnumerator ChangeLightingPower()
        {
            while (true)
            {
                yield return new WaitForSeconds(LightingDelay);
                while (CurrentPower < MaxLightingPower)
                {
                    float newPower = CurrentPower + PowerChangeSpeed;
                    if (newPower > MaxLightingPower)
                        newPower = MaxLightingPower;
                    SetLightingPower(newPower);
                    yield return new WaitForEndOfFrame();
                }
                while (CurrentPower > 0)
                {
                    float newPower = CurrentPower - PowerChangeSpeed;
                    if (newPower < 0)
                        newPower = 0;
                    SetLightingPower(newPower);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        private void OnActivationInteraction()
        {
            if (CurrentChanger == null)
            {
                CurrentChanger = StartCoroutine(ChangeLightingPower());
            }
        }
        private void Awake()
        {
            string ExcSrc = "InteractivaObjShadingChanger.Awake()";

            if (OwnerMeshes == null || OwnerMeshes.Length == 0)
                throw new AIADException("Missing meshes.", ExcSrc);
            if (LightingDelay < 0)
                throw new AIADException("LightingDelay must be equal or greater than zero.", ExcSrc);
            if (PowerChangeSpeed <= 0)
                throw new AIADException("PowerChangeSpeed must be greater than zero.", ExcSrc);

            IInteractiveObject obj = Owner as IInteractiveObject;

            if (obj == null)
                throw new AIADException("Missing Owner",ExcSrc);

            obj.TurnOnInteractionEvent += OnActivationInteraction;
            obj.TurnOffInteractionEvent += OnDeactivationInteraction;
            if (obj.CanInteract_)
                OnActivationInteraction();
        }
    }
}
