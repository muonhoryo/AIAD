using AIAD.Exceptions;
using AIAD.SL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class HitPointModule : MonoBehaviour,IHitPointModule
    {
        public event Action<int> HPCountHasChangedEvent=delegate { };
        public event Action<int> LowHPCountHasAchievedEvent = delegate { };
        public event Action HPCountHasBecomeZeroEvent = delegate { };
        [SerializeField] private string SLScriptsIDS_LowHPAchivied;
        [SerializeField] private string SLScriptsIDS_HPBecameZero;


        private int CurrentHP;

        private void Awake()
        {
            string ExcSrc = "HitPointModule.Start()";
            if (ExternalConsts.Consts_.MaxHP <= 0)
                throw new AIADException("MaxHP must be greater than zero",ExcSrc );

            if (ExternalConsts.Consts_.LowHP <= 0)
                throw new AIADException("LowHPLevel must be greater than zero", ExcSrc);
            if (ExternalConsts.Consts_.LowHP >= ExternalConsts.Consts_.MaxHP)
                throw new AIADException("LowHPLevel must be less than MaxHP", ExcSrc);

            CurrentHP = ExternalConsts.Consts_.MaxHP;

            if (!string.IsNullOrEmpty(SLScriptsIDS_LowHPAchivied))
                LowHPCountHasAchievedEvent += (i) => SLScriptsIDS_LowHPAchivied.RunSLScripts();
            if (!string.IsNullOrEmpty(SLScriptsIDS_HPBecameZero))
                HPCountHasBecomeZeroEvent += () => SLScriptsIDS_HPBecameZero.RunSLScripts();
        }

        public void SetPointCount(int count)
        {
            if (count > ExternalConsts.Consts_.MaxHP)
                count = ExternalConsts.Consts_.MaxHP;
            CurrentHP = count;
            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                HPCountHasBecomeZeroEvent();
            }
            else if (CurrentHP <= ExternalConsts.Consts_.LowHP)
                LowHPCountHasAchievedEvent(CurrentHP);
            HPCountHasChangedEvent(CurrentHP);
        }
        public void ChangePointCount(int value)
        {
            SetPointCount(CurrentHP+ value);
        }

        public int CurrentHP_ => CurrentHP;
        public int MaxHP_ => ExternalConsts.Consts_.MaxHP;
        public int LowHPLimit_ => ExternalConsts.Consts_.LowHP;
    }
}
