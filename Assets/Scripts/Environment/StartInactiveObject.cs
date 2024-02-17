using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class StartInactiveObject : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
    }
}
