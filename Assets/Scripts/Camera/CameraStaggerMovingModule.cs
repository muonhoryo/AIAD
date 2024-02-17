
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class CameraStaggerMovingModule : MonoBehaviour,ICameraStaggerValueSource
    {
        [SerializeField] private MonoBehaviour MovingModuleComponent;

        [SerializeField] private Animator Animator;
        private IMovingModule MovingModule;

        [SerializeField] private string RunAnimationName;
        [SerializeField] private string AbortAnimationName;

        public Vector3 StaggerRotationOffset;

        private void StartMovingAction(Vector3 direction)
        {
            Animator.SetBool(RunAnimationName, true);
        }
        private void StopMovingAction()
        {
            Animator.SetBool(RunAnimationName, false);
        }

        private void TurnOffModule()
        {
            if (MovingModule != null)
            {
                MovingModule.StartMovingEvent -= StartMovingAction;
                MovingModule.StopMovingEvent -= StopMovingAction;
                Animator.SetTrigger(AbortAnimationName);
            }
        }
        private void TurnOnModule()
        {
            if (MovingModule == null)
            {
                throw new AIADMissingModuleException("MovingModule", "ViewStaggerModule.Awake()");
            }
            MovingModule.StartMovingEvent += StartMovingAction;
            MovingModule.StopMovingEvent += StopMovingAction;
        }

        private void Awake()
        {
            MovingModule = MovingModuleComponent as IMovingModule;
            TurnOnModule();
        }
        private void OnEnable()
        {
            TurnOnModule();
        }

        private void OnDestroy()
        {
            TurnOffModule();
        }
        private void OnDisable()
        {
            TurnOffModule();
        }

        Vector3 ICameraStaggerValueSource.StaggerRotationOffset_ => StaggerRotationOffset;
    }
}
