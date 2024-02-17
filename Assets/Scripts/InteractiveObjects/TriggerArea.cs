using AIAD.Player;
using AIAD.SL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class TriggerArea : MonoBehaviour
    {
        public event Action EnterAreaEvent=delegate { };
        public event Action ExitAreaEvent=delegate { };

        [SerializeField] private string EnterAreaScriptsID;
        [SerializeField] private string ExitAreaScriptsID;

        private void Awake()
        {
            if (!string.IsNullOrEmpty(EnterAreaScriptsID))
            {
                EnterAreaEvent += () => EnterAreaScriptsID.RunSLScripts();
            }
            if (!string.IsNullOrEmpty(ExitAreaScriptsID))
            {
                ExitAreaEvent += () => ExitAreaScriptsID.RunSLScripts();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerController ctrlr = other.GetComponentInParent<PlayerController>();
            if(ctrlr != null)
            {
                EnterAreaEvent();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            PlayerController ctrlr = other.GetComponentInParent<PlayerController>();
            if(ctrlr != null)
            {
                ExitAreaEvent();
            }
        }
    }
}
