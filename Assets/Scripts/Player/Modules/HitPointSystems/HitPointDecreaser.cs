using AIAD.Exceptions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class HitPointDecreaser : MonoBehaviour,ILockableModule
    {
        [SerializeField] private MonoBehaviour HPModuleComponent;

        [SerializeField][Range(0,10)] private float TimeInterval;
        [SerializeField][Range(1,100)] private int DecreasedPointCount;
        [SerializeField] private bool IsActiveOnStart=false;

        private IHitPointModule HPModule;

        private bool IsWorking = false;

        private void Awake()
        {
            string ExcSrc = "HitPointDecreaser.Awake()";

            if (TimeInterval <= 0)
                throw new AIADException("TimeInterval must be greater than zero.", ExcSrc);
            if (DecreasedPointCount <= 0)
                throw new AIADException("DecreasedPointCount must be greater than zero", ExcSrc);
            HPModule = HPModuleComponent as IHitPointModule;
            if (HPModule == null)
                throw new AIADMissingModuleException("HPModule", ExcSrc);

            if(IsActiveOnStart)
                StartModuleWorking();
        }
        private IEnumerator DecreaseStep()
        {
            while (true)
            {
                yield return new WaitForSeconds(TimeInterval);
                HPModule.ChangePointCount(-DecreasedPointCount);
            }
        }
        public void StartModuleWorking()
        {
            StartCoroutine(DecreaseStep());
            IsWorking = true;
            HPModule.HPCountHasBecomeZeroEvent += StopModuleWorking;
        }
        public void StopModuleWorking()
        {
            StopAllCoroutines();
            IsWorking = false;
            HPModule.HPCountHasBecomeZeroEvent -= StopModuleWorking;
        }

        public void Lock() => StopModuleWorking();
        public void Unlock() => StartModuleWorking();
        bool ILockableModule.IsLocked_ => !IsWorking;
    }
}
