using AIAD.SL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class AnimationEvents : MonoBehaviour
    {
        public event Action<int> AnimationHasEndedEvent=delegate { };

        public void AnimationHasEnded(int identifier)
        {
            AnimationHasEndedEvent(identifier);
        }
        public void RunSLScripts(string IDs)
        {
            if (!string.IsNullOrEmpty(IDs))
                IDs.RunSLScripts();
        }
    }
}
