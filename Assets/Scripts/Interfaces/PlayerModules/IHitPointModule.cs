using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public interface IHitPointModule
    {
        public event Action<int> HPCountHasChangedEvent;
        public event Action<int> LowHPCountHasAchievedEvent;
        public event Action HPCountHasBecomeZeroEvent;
        public int CurrentHP_ { get; }
        public int MaxHP_ { get; }
        public int LowHPLimit_ { get; }
        public void SetPointCount(int count);
        /// <summary>
        /// Increase or decrease HP count by value
        /// </summary>
        /// <param name="value"></param>
        public void ChangePointCount(int value);
    }
}
