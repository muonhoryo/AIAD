using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public interface IMovingModule
    {
        public event Action<Vector3> StartMovingEvent;
        public event Action<Vector3> ChangeMovingDirectionEvent;
        public event Action StopMovingEvent;

        public bool IsMoving_ { get; }
        public Vector3 MovingDirection_ { get; }
        public Vector3 CurrentMovableObjectPosition_ { get; }

        public void SetMovingDirection(Vector2 inputDirection)
        {
            if (inputDirection != Vector2.zero)
            {
                if (!IsMoving_)
                {
                    StartMovingAction(inputDirection);
                }
                else
                {
                    SetMovingDirectionAction(inputDirection);
                }
            }
        }
        public void StopMoving()
        {
            if (IsMoving_)
            {
                StopMovingAction();
            }
        }

        protected void StartMovingAction(Vector2 direction);
        protected void SetMovingDirectionAction(Vector2 direction);
        protected void StopMovingAction();
    }

}
