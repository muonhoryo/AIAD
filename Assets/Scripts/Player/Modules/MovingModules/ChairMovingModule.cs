using System;
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class ChairMovingModule : MonoBehaviour,IMovingModule,ILockableModule
    {
        public event Action<Vector3> StartMovingEvent = delegate { };
        public event Action<Vector3> ChangeMovingDirectionEvent = delegate { };
        public event Action StopMovingEvent = delegate { };

        [SerializeField] private Transform MovableObj;
        [SerializeField] private MonoBehaviour MovDirCalculatorComponent;

        private IMovDirCalcModule MovDirCalculator;

        [SerializeField] private Vector3 MovingAxis;
        [SerializeField] private Vector3 StartPointLocal;
        [SerializeField] private Vector3 EndPointLocal;

        private float MovingDirection;
        private Vector2 HorizontalMovingAxis;
        private bool IsWorking = true;
        private float MovingLineSize;

        private void Awake()
        {
            string ExcSrc = "ChairMovingModule.Awake()";

            if (MovableObj == null)
                throw new AIADException("Missing MovableObject.", ExcSrc);
            MovDirCalculator = MovDirCalculatorComponent as IMovDirCalcModule;
            if (MovDirCalculator == null)
                throw new AIADException("Missing MovDirCalculator");

            enabled = false;
            HorizontalMovingAxis = new Vector2(MovingAxis.x, MovingAxis.z);
            MovingLineSize = (EndPointLocal - StartPointLocal).magnitude;
        }
        private void FixedUpdate()
        {
            float stepSize = MovingDirection * ExternalConsts.Consts_.PlayerChairMovingSpeed;
            Vector3 step= MovingAxis * stepSize;
            Vector3 direction = MovingAxis * MovingDirection;
            void EndMoving(Vector3 endPoint)
            {
                MovableObj.localPosition = endPoint;
                StopMoving();
            }
            if (Vector3.Dot(direction, (EndPointLocal-StartPointLocal ).normalized) > 0)
            {
                Vector3 objPos = MovableObj.localPosition - StartPointLocal;
                float distDiff = MovingLineSize - objPos.magnitude;
                if (distDiff <Math.Abs(stepSize))
                {
                    EndMoving(EndPointLocal);
                    return;
                }
            }
            else
            {
                Vector3 objPos = MovableObj.localPosition - EndPointLocal;
                float distDiff = MovingLineSize - objPos.magnitude;
                if (distDiff <MathF.Abs(stepSize))
                {
                    EndMoving(StartPointLocal);
                    return;
                }
            }
            MovableObj.localPosition += step;
        }
        private void OnEnable()
        {
            if (!IsWorking)
            {
                enabled = false;
            }
        }
        private void StartMoving(Vector2 direction)
        {
            SetMovingDirection(direction);
            enabled = true;
            if (IsWorking)
                StartMovingEvent(MovingAxis * MovingDirection);
        }
        private void SetMovingDirection(Vector2 direction)
        {
            Vector3 calculatedDir=MovDirCalculator.GetDirection(direction);
            MovingDirection = new Vector3
                (MovingAxis.x * calculatedDir.x,
                0,
                MovingAxis.z * calculatedDir.z).magnitude;
            if (Vector2.Dot(new Vector2(calculatedDir.x, calculatedDir.z), HorizontalMovingAxis) < 0)
                MovingDirection *= -1;
            ChangeMovingDirectionEvent(MovingAxis * MovingDirection);
        }
        private void StopMoving()
        {
            enabled = false;
            MovingDirection = 0;
            StopMovingEvent();
        }

        public Vector3 CurrentMovableObjectPosition_ => MovableObj.position;

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
        public bool IsLocked_ => !IsWorking;

        Vector3 IMovingModule.MovingDirection_ => HorizontalMovingAxis*MovingDirection;
    }
}
