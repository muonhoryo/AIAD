using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    /// <summary>
    /// Calculate moving direction for IMovingModule based on player inputs
    /// </summary>
    public interface IMovDirCalcModule
    {
        public Vector3 GetDirection(float HorizontalAxisValue, float VerticalAxisValue);
        public Vector3 GetDirection(Vector2 movingDirection)=>
            GetDirection(movingDirection.x,movingDirection.y);
    }
}
