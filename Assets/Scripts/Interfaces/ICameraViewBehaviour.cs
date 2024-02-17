using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public interface ICameraViewBehaviour 
    {
        /// <summary>
        /// First argument - rotation's value, second - rotation around y
        /// </summary>
        public event Action<Vector3,float> ChangeViewRotationEvent;
        /// <summary>
        /// First argument - perspective direction, second - horizontal direction
        /// </summary>
        public event Action<Vector3, Vector2> ChangeViewDirectionEvent;
        public Vector3 CurrentViewDirection_ { get; }
        public Vector2 CurrentHorizontalViewDirection_ { get; }
        public void Rotate(Vector3 rotation);
        public void SetRotation(Vector3 rotation);
        public void SetXRotation(float xRotation);
        public void SetYRotation(float yRotation);
        public void SetZRotation(float zRotation);

        public void Rotate(Vector2 rotation)
        {
            Rotate(new Vector3(rotation.x, rotation.y, 0));
        }
        public void SetRotation(Vector2 rotation)
        {
            SetRotation(new Vector3(rotation.x, rotation.y, 0));
        }
    }
}
