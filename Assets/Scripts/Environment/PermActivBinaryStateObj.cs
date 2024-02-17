using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class PermActivBinaryStateObj : MonoBehaviour,IBinaryActivStateObj
    {
        public event Action ActivationEvent = delegate { };
        public event Action DeactivationEvent= delegate { };

        public bool IsActive_ => true;

        private void Start()
        {
            ActivationEvent();
        }
    }
}

