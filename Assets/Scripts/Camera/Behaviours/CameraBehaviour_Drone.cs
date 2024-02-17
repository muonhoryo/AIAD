using MuonhoryoLibrary.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class CameraBehaviour_Drone : MonoBehaviour, CameraBehaviourSelector.ITurnedOffCameraBehaviours
    {
        public event Action<Vector3, float> ChangeViewRotationEvent = delegate { };
        public event Action<Vector3,Vector2> ChangeViewDirectionEvent = delegate { };

        [SerializeField] private Transform ViewObject;

        public void Rotate(Vector3 rotation)
        {
            ViewObject.Rotate(rotation);
            ViewObject.eulerAngles = ViewObject.eulerAngles;
            ChangeViewRotationEvent(ViewObject.eulerAngles, -ViewObject.eulerAngles.y + 90);
            ChangeViewDirectionEvent(CurrentViewDirection_, CurrentHorizontalViewDirection_);
        }
        public void SetRotation(Vector3 rotation)
        {
            ViewObject.eulerAngles = rotation;
            ChangeViewRotationEvent(ViewObject.eulerAngles, -ViewObject.eulerAngles.y + 90);
            ChangeViewDirectionEvent(CurrentViewDirection_, CurrentHorizontalViewDirection_);
        }
        public void SetXRotation(float rotation)
        {
            SetRotation(new Vector3(rotation, ViewObject.eulerAngles.y, ViewObject.eulerAngles.z));
        }
        public void SetYRotation(float rotation)
        {
            SetRotation(new Vector3(ViewObject.eulerAngles.x, rotation, ViewObject.eulerAngles.z));
        }
        public void SetZRotation(float rotation)
        {
            SetRotation(new Vector3(ViewObject.eulerAngles.x, ViewObject.eulerAngles.y, rotation));
        }

        public Vector3 CurrentViewDirection_ => ViewObject.eulerAngles.DirectionFromAngle3D();
        public Vector2 CurrentHorizontalViewDirection_ => (-ViewObject.eulerAngles.y + 90).DirectionOfAngle();


        void CameraBehaviourSelector.ITurnedOffCameraBehaviours.TurnOn() => enabled = true;
        void CameraBehaviourSelector.ITurnedOffCameraBehaviours.TurnOff() => enabled = false;
    }
}
