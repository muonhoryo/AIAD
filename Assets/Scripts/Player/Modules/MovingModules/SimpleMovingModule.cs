
using AIAD.Exceptions;
using System;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class SimpleMovingModule : MonoBehaviour, IMovingModule,ILockableModule
    {
        public event Action<Vector3> StartMovingEvent = delegate { };
        public event Action<Vector3> ChangeMovingDirectionEvent = delegate { };
        public event Action StopMovingEvent = delegate { };

        [SerializeField] private Rigidbody RGBody;
        [SerializeField] private MonoBehaviour MovDirCalculactorComponent;

        private IMovDirCalcModule MovDirCalculator;

        private Vector3 MovingDirection= Vector3.zero;
        private bool IsWorking = true;

        private void Awake()
        {
            string ExcSrc = "SimpleMovingModule.Awake()";

            MovDirCalculator = MovDirCalculactorComponent as IMovDirCalcModule;

            if (MovDirCalculator == null)
                throw new AIADException("MovDirCalculator", ExcSrc);

            if (RGBody == null)
                throw new AIADException("RGBody wasn't initialized", ExcSrc);
            enabled = false;
        }
        private float Speed_ => ExternalConsts.Consts_.PlayerStandMovingSpeed;
        private void FixedUpdate()
        {
            RGBody.AddForce(MovingDirection* Speed_, ForceMode.Force);
            if (RGBody.velocity.magnitude > Speed_)
                RGBody.velocity = RGBody.velocity.normalized * Speed_;
        }
        private void OnEnable()
        {
            if (!IsWorking)
                enabled = false;
        }
        private void StartMoving(Vector2 direction)
        {
            SetMovingDirection(direction);
            enabled = true;
            if (IsWorking)
                StartMovingEvent(MovingDirection);
        }
        private void SetMovingDirection(Vector2 direction)
        {
            MovingDirection =MovDirCalculator.GetDirection(direction);
            ChangeMovingDirectionEvent(MovingDirection);
        }
        private void StopMoving()
        {
            enabled = false;
            RGBody.velocity = Vector3.zero;
            StopMovingEvent();
        }

        public Vector3 CurrentMovableObjectPosition_ => RGBody.transform.position;

        void IMovingModule.StartMovingAction(Vector2 direction) => StartMoving(direction);
        void IMovingModule.SetMovingDirectionAction(Vector2 direction) => SetMovingDirection(direction);
        void IMovingModule.StopMovingAction() => StopMoving();

        void ILockableModule.Lock()
        {
            IsWorking = false;
            StopMoving();
        }
        void ILockableModule.Unlock()
        {
            IsWorking = true;
        }

        bool IMovingModule.IsMoving_ { get => enabled; }
        bool ILockableModule.IsLocked_ => !IsWorking;

        Vector3 IMovingModule.MovingDirection_ => MovingDirection;
    }
}
