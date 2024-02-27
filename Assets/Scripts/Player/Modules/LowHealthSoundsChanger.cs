
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class LowHealthSoundsChanger : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour HPModuleComponent;
        [SerializeField] private AnimationCurve[] ChangingSoundsVolumeFuncs;
        [SerializeField] private AudioSource[] SoundsSources;

        private IHitPointModule HPModule;
        private void HPHasChangedAction(int newHP)
        {
            float key= Mathf.Clamp01(1 - ((float)newHP / HPModule.LowHPLimit_));
            for(int i = 0; i < SoundsSources.Length; i++)
            {
                SoundsSources[i].volume = ChangingSoundsVolumeFuncs[i].Evaluate(key);
            }
        }
        private void Awake()
        {
            string ExcSrc = "LowHealthSoundsChanger.Awake()";

            HPModule = HPModuleComponent as IHitPointModule;

            if (HPModule == null)
                throw new AIADException("Missing HPModule.", ExcSrc);
            if (ChangingSoundsVolumeFuncs == null || ChangingSoundsVolumeFuncs.Length <= 0)
                throw new AIADException("Volume Funcs's array is null or empty.", ExcSrc);
            if (SoundsSources == null || SoundsSources.Length <= 0)
                throw new AIADException("Sounds sources's array is null or empty.", ExcSrc);
            if (ChangingSoundsVolumeFuncs.Length != SoundsSources.Length)
                throw new AIADException("Arrays's lengths must be equal.", ExcSrc);

            HPModule.HPCountHasChangedEvent += HPHasChangedAction;
        }
    }
}
