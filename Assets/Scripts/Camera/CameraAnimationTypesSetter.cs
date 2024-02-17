using AIAD.Exceptions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class CameraAnimationTypesSetter : StateMachineBehaviour
    {
        [SerializeField] private bool IsAnimatePosition = true;
        [SerializeField] private bool IsAnimateRotation = true;
        [SerializeField] private bool IsGlobalAnimRotation_X = true;
        [SerializeField] private bool IsGlobalAnimRotation_Y = true;
        [SerializeField] private bool IsGlobalAnimRotation_Z = true;
        [SerializeField] private bool IsGlobalAnimPosition_X = true;
        [SerializeField] private bool IsGlobalAnimPosition_Y = true;
        [SerializeField] private bool IsGlobalAnimPosition_Z = true;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.TryGetComponent(out CameraBehaviour_Animation owner))
                throw new AIADException("Missing CameraBehaviour_Animation.", "CameraAnimationLocalCenterSetter.OnStateEnter()");

            owner.SetAnimationParams(IsAnimatePosition, IsAnimateRotation, IsGlobalAnimPosition_X, IsGlobalAnimPosition_Y, IsGlobalAnimPosition_Z,
                IsGlobalAnimRotation_X, IsGlobalAnimRotation_Y, IsGlobalAnimRotation_Z);
        }
    }
}
