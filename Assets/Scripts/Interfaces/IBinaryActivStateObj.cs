using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public interface IBinaryActivStateObj
    {
        public event Action ActivationEvent;
        public event Action DeactivationEvent;

        public bool IsActive_ { get; }
    }
}
