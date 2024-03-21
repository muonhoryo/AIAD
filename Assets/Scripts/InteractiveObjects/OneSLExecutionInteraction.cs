
using AIAD.Exceptions;
using AIAD.SL;
using UnityEngine;

namespace AIAD
{
    public sealed class OneSLExecutionInteraction : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour InteractiveObjComponent;
        [SerializeField] private string SLScriptName;

        private void Awake()
        {
            string ExcSrc = "OneSLExecutionInteraction.Awake()";

            IInteractiveObject obj=InteractiveObjComponent as IInteractiveObject;
            void InteractionAction()
            {
                SLScriptName.RunSLScripts();
                obj.InteractionEvent -= InteractionAction;
            }

            if (obj == null)
                throw new AIADException("Missing InteractiveObjComponent.", ExcSrc);
            if (string.IsNullOrEmpty(SLScriptName))
                throw new AIADException("Missing SLScriptName.", ExcSrc);

            obj.InteractionEvent += InteractionAction;
            Destroy(this);
        }
    }
}
