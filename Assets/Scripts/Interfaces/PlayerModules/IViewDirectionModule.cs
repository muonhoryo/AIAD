using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public interface IViewDirectionModule
    {
        /// <summary>
        /// First argument - view direction, second - horizontal view direction
        /// </summary>
        public event Action<Vector3,Vector2> ChangeViewDirectionEvent;
        public Vector2 CurrentHorizontalViewDirection_ { get; }
        public Vector3 CurrentViewDirection_ { get; }
        public void ChangeViewDirection(float XMoving, float YMoving);
    }
}
