using AIAD.Exceptions;
using AIAD.Player.COM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class DroneMovingModule : MonoBehaviour,IMovingModule,ILockableModule
    {
        public event Action<Vector3> StartMovingEvent = delegate { };
        public event Action<Vector3> ChangeMovingDirectionEvent = delegate { };
        public event Action StopMovingEvent = delegate { };

        [SerializeField] private Rigidbody RGBody;
        [SerializeField] private Transform BoundsCenter;
        [SerializeField] private MonoBehaviour MovDirCalculactorComponent;

        private IMovDirCalcModule MovDirCalculator;

        [SerializeField] private float BoundsRadius;

        private Vector3 MovingDirection = Vector3.zero;
        private bool IsActive = true;
        private bool IsWorking = false;

        private void Awake()
        {
            string ExcSrc = "DroneMovingModule.Awake()";

            MovDirCalculator = MovDirCalculactorComponent as IMovDirCalcModule;

            if (MovDirCalculator == null)
                throw new AIADException("MovDirCalculator", ExcSrc);
            if (RGBody == null)
                throw new AIADException("RGBody wasn't initialized", ExcSrc);
            if (BoundsCenter == null)
                throw new AIADException("Missing BoundsCenter object.", ExcSrc);

            IsWorking = false;
        }
        private float Speed_ => ExternalConsts.Consts_.DroneMovingSpeed;
        private void FixedUpdate()
        {
            if (IsWorking)
            {
                RGBody.AddForce(MovingDirection * Speed_, ForceMode.Force);
                if (RGBody.velocity.magnitude > Speed_)
                    RGBody.velocity = RGBody.velocity.normalized * Speed_;
            }
            if (Vector3.Distance(RGBody.gameObject.transform.position, BoundsCenter.transform.position) > BoundsRadius)
            {
                Vector3 center2owner =  RGBody.gameObject.transform.position - BoundsCenter.transform.position;
                RGBody.gameObject.transform.position = BoundsCenter.transform.position + ( center2owner.normalized * BoundsRadius);
            }
        }
        private void StartMoving(Vector2 direction)
        {
            SetMovingDirection(direction);
            IsWorking = true;
            StartMovingEvent(MovingDirection);
        }
        private void SetMovingDirection(Vector2 direction)
        {
            MovingDirection = MovDirCalculator.GetDirection(direction);
            ChangeMovingDirectionEvent(MovingDirection);
        }
        private void StopMoving()
        {
            IsWorking = false;
            StopMovingEvent();
        }

        public Vector3 CurrentMovableObjectPosition_ => RGBody.transform.position;

        void IMovingModule.StartMovingAction(Vector2 direction) => StartMoving(direction);
        void IMovingModule.SetMovingDirectionAction(Vector2 direction) => SetMovingDirection(direction);
        void IMovingModule.StopMovingAction() => StopMoving();

        void ILockableModule.Lock()
        {
            IsActive = false;
            StopMoving();
        }
        void ILockableModule.Unlock()
        {
            IsActive = true;
        }

        bool IMovingModule.IsMoving_ { get => IsWorking; }
        bool ILockableModule.IsLocked_ => !IsActive;

        Vector3 IMovingModule.MovingDirection_ => MovingDirection;
    }
}
