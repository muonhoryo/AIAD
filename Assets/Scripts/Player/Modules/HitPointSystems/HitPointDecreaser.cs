using AIAD.Exceptions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class HitPointDecreaser : MonoBehaviour,ILockableModule
    {
        private const int MaxDecreasedPointCount = 100;

        [SerializeField] private MonoBehaviour HPModuleComponent;

        [SerializeField][Range(0,10)] private float TimeInterval;
        [SerializeField][Range(1, MaxDecreasedPointCount)] private int DecreasedPointCount;
        [SerializeField] private bool IsActiveOnStart=false;

        public float DecreasingTimeInterval_ => TimeInterval;
        public int DecreasingPointCount_ => DecreasedPointCount;

        public IHitPointModule HPModule_ { get; private set; }

        private bool IsWorking = false;

        private void Awake()
        {
            string ExcSrc = "HitPointDecreaser.Awake()";

            if (TimeInterval <= 0)
                throw new AIADException("TimeInterval must be greater than zero.", ExcSrc);
            if (DecreasedPointCount <= 0)
                throw new AIADException("DecreasedPointCount must be greater than zero", ExcSrc);
            HPModule_ = HPModuleComponent as IHitPointModule;
            if (HPModule_ == null)
                throw new AIADMissingModuleException("HPModule_", ExcSrc);

            if(IsActiveOnStart)
                StartModuleWorking();
        }
        private IEnumerator DecreaseStep()
        {
            while (true)
            {
                yield return new WaitForSeconds(TimeInterval);
                HPModule_.ChangePointCount(-DecreasedPointCount);
            }
        }
        public void StartModuleWorking()
        {
            StartCoroutine(DecreaseStep());
            IsWorking = true;
            HPModule_.HPCountHasBecomeZeroEvent += StopModuleWorking;
        }
        public void StopModuleWorking()
        {
            StopAllCoroutines();
            IsWorking = false;
            HPModule_.HPCountHasBecomeZeroEvent -= StopModuleWorking;
        }
        public void SetDecreasingSpeed(float pointPerSecond)
        {
            if (pointPerSecond <= 0)
                throw new AIADException("Speed must be greater than null.", "HitPointDecreaser.SetDecreasingSpeed()");

            float newDecPntCount = pointPerSecond * TimeInterval;
            if (newDecPntCount > MaxDecreasedPointCount)
            {
                TimeInterval = MaxDecreasedPointCount / pointPerSecond;
                DecreasedPointCount = MaxDecreasedPointCount;
                return;
            }
            else
            {
                float roundedCount = Mathf.Round(newDecPntCount);
                if (roundedCount != newDecPntCount)
                {
                    TimeInterval *= (newDecPntCount / roundedCount);
                    DecreasedPointCount = (int)roundedCount;
                    return;
                }
            }
            DecreasedPointCount = (int)newDecPntCount;
        }

        public void Lock() => StopModuleWorking();
        public void Unlock() => StartModuleWorking();
        bool ILockableModule.IsLocked_ => !IsWorking;
    }
}
