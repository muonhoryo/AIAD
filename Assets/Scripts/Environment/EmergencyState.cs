using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class EmergencyState : MonoBehaviour
    {
        public event Action OnEmergencyEvent = delegate { };

        public void SetEmergency()
        {
            OnEmergencyEvent();
        }
    }
}
