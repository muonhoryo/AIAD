
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD
{
    public sealed class CameraAnimationLocalCenterSetter : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.TryGetComponent(out CameraBehaviour_Animation owner))
                throw new AIADException("Missing CameraBehaviour_Animation.", "CameraAnimationLocalCenterSetter.OnStateEnter()");

            owner.SetCentersAsCurrent();
        }
    }
}
