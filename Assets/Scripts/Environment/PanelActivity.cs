using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIAD.Exceptions;

namespace AIAD
{
    public sealed class PanelActivity : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour OwnerComponent;
        [SerializeField] private GameObject ActivatedObject;


        private void DeactivationAction()
        {
            ActivatedObject.SetActive(false);
        }
        private void ActivationAction()
        {
            ActivatedObject.SetActive(true);
        }
        private void Awake()
        {
            string ExcSrc = "PanelActivity.Awake()";

            IBinaryActivStateObj panel = OwnerComponent as IBinaryActivStateObj;

            if (panel == null)
                throw new AIADException("Missing Panel", ExcSrc);
            if (ActivatedObject == null)
                throw new AIADException("Missing ActivatedObject", ExcSrc);

            panel.ActivationEvent += ActivationAction;
            panel.DeactivationEvent += DeactivationAction;
        }
    }
}
