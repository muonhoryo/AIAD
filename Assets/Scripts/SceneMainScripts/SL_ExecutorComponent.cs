using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.SL
{
    public sealed class SL_ExecutorComponent : MonoBehaviour
    {
        private void Awake()
        {
            SL_Executor.ExecutorComponent = this;
        }
    }
}
