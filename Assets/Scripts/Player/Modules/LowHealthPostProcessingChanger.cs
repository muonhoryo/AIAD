
using AIAD.Exceptions;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace AIAD.Player.COM
{
    public sealed class LowHealthPostProcessingChanger : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour HPModuleComponent;
        [SerializeField] private PostProcessVolume[] IncreasingVolumes;
        [SerializeField] private PostProcessVolume[] DecreasingVolumes;

        private IHitPointModule HPModule;


        private void HPHasChangedAction(int newHPCount)
        {
            float weight =Mathf.Clamp01(1 - ((float)newHPCount /HPModule.LowHPLimit_));
            foreach (var volume in IncreasingVolumes)
            {
                volume.weight= weight;
            }
            weight = 1-weight;
            foreach(var volume in DecreasingVolumes)
            {
                volume.weight= weight;
            }
        }
        private void Awake()
        {
            HPModule = HPModuleComponent as IHitPointModule;

            if (HPModule == null)
                throw new AIADException("Missing HPModule_.", "LoewHealthPostProcessingChanger.Awake()");

            HPModule.HPCountHasChangedEvent += HPHasChangedAction;
        }
    }
}
