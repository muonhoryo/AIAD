using AIAD.Player.COM;
using MuonhoryoLibrary.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace AIAD
{
    public sealed class CameraDirectionBehaviour_Human : MonoBehaviour, CameraBehaviourSelector.ITurnedOffCameraBehaviours
    {
        public event Action<Vector3,float> ChangeViewRotationEvent = delegate { };
        public event Action<Vector3, Vector2> ChangeViewDirectionEvent = delegate { };

        [SerializeField] private Transform ViewObject;
        [SerializeField] private MonoBehaviour CameraStaggerComponent;

        private ICameraStaggerValueSource CameraStagger;

        private Vector3 CurrentViewRotation = Vector3.zero;

        private void Awake()
        {
            CameraStagger = CameraStaggerComponent as ICameraStaggerValueSource;
        }

        public void Rotate(Vector3 rotation)
        {
            float newY = (CurrentViewRotation.y + rotation.y)%360;
            float newX = (CurrentViewRotation.x + rotation.x+360)%360;
            //Limit vertical rotation
            if (newX > 90 && newX < 270)
            {
                newX = newX < 180 ? 90 : -90;
            }
            SetRotation(new Vector3(newX, newY,0));
        }
        public void SetRotation(Vector3 rotation)
        {
            CurrentViewRotation = rotation;
            LateUpdate();
            ChangeViewRotationEvent(CurrentViewRotation,CurrentViewRotation.y);
            ChangeViewDirectionEvent(CurrentViewDirection_, CurrentHorizontalViewDirection_);
        }
        public void SetXRotation(float rotation)
        {
            SetRotation(new Vector3(rotation, CurrentViewRotation.y, CurrentViewRotation.z));
        }
        public void SetYRotation(float rotation)
        {
            SetRotation(new Vector3(CurrentViewRotation.x, rotation, CurrentViewRotation.z));
        }
        public void SetZRotation(float rotation)
        {
            SetRotation(new Vector3(CurrentViewRotation.x, CurrentViewRotation.y, rotation));
        }

        private void LateUpdate()
        {
            ViewObject.eulerAngles = CurrentViewRotation + (CameraStagger != null ? CameraStagger.StaggerRotationOffset_ : Vector3.zero);
        }

        public Vector3 CurrentViewDirection_ => CurrentViewRotation.DirectionFromAngle3D();
        public Vector2 CurrentHorizontalViewDirection_ => (-ViewObject.eulerAngles.y + 90).DirectionOfAngle();

        void CameraBehaviourSelector.ITurnedOffCameraBehaviours.TurnOn() => enabled = true;
        void CameraBehaviourSelector.ITurnedOffCameraBehaviours.TurnOff() => enabled = false;
    }
}
