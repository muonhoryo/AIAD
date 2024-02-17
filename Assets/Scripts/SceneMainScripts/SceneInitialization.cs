using AIAD.Exceptions;
using AIAD.SL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class SceneInitialization : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour MainCameraBehaviourComponent;
        [SerializeField] private string SLScriptsIDS_Start;

        private void Awake()
        {
            Registry.MainCameraBehaviour = MainCameraBehaviourComponent as ICameraViewBehaviour;
            if (Registry.MainCameraBehaviour == null)
                throw new AIADException("Missing MainCameraBehaviour.", "SceneInitialization.Awake()");
        }
        private void Start()
        {
            if (!string.IsNullOrEmpty(SLScriptsIDS_Start))
            {
                SLScriptsIDS_Start.RunSLScripts();
            }
            Destroy(this);
        }
    }
}
