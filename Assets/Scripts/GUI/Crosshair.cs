using AIAD.Player.COM;
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD
{
    public sealed class Crosshair : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour InteractionModuleComponent;
        [SerializeField] private string SelectionAnimName;
        [SerializeField] private Animator Animator;

        private void Awake()
        {
            string ExcSrc = "Crosshair.Awake()";

            IInteractionModule module = InteractionModuleComponent as IInteractionModule;

            if (module == null)
                throw new AIADException("Missing interaction module.", ExcSrc);
            if (string.IsNullOrEmpty(SelectionAnimName))
                throw new AIADException("SelectionAnimName cannot be null or empty.", ExcSrc);
            if (Animator == null)
                throw new AIADException("Missing Animator.", ExcSrc);

            module.InteractiveObjHasBeenFoundEvent += (i) =>
            {
                if (i.CanInteract_)
                    Animator.SetBool(SelectionAnimName, true);
            };
            module.InteractiveObjHasBeenLostEvent += () => Animator.SetBool(SelectionAnimName, false);
        }
    }
}
