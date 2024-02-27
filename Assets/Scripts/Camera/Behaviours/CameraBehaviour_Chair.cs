using MuonhoryoLibrary.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AIAD
{
    public sealed class CameraBehaviour_Chair : MonoBehaviour, CameraBehaviourSelector.ITurnedOffCameraBehaviours
    {
        public event Action<Vector3, float> ChangeViewRotationEvent = delegate { };
        public event Action<Vector3, Vector2> ChangeViewDirectionEvent = delegate { };
        public event Action<float> ChangeChairDirectionEvent = delegate { };
        public event Action<int> StartChairRotationEvent = delegate { };
        public event Action StopChairRotationEvent = delegate { };

        [SerializeField] private Transform ViewObject;
        [SerializeField] private MonoBehaviour CameraStaggerComponent;

        private ICameraStaggerValueSource CameraStagger;

        [SerializeField][Range(0.001f,10000)] private float ChairDirectionChangeSpeed;
        [SerializeField][Range(0.001f, 179.9f)] private float ChairFreeRadius;  //Size of sector, where can be view around chair
        [SerializeField][Range(0.002f, 179.8f)] private float ChairBorderLimit;   //Size of sector, which dont activate chair rotation
        [SerializeField][Range(0, 360)] private float StartChairDirection;

        private float CurrentChairDirection = 0;
        private Vector3 CurrentViewRotation = Vector3.zero;
        private int ChairRotationSide = 0;

        private void Awake()
        {
            CameraStagger = CameraStaggerComponent as ICameraStaggerValueSource;
        }

        public void Rotate(Vector3 rotation)
        {
            float newY = (CurrentViewRotation.y + rotation.y) % 360;
            float newX = (CurrentViewRotation.x + rotation.x + 360) % 360;
            //Limit vertical rotation
            if (newX > 90 && newX < 270)
            {
                newX = newX < 180 ? 90 : -90;
            }
            SetRotation(new Vector3(newX, newY, 0));
        }

        /// <summary>
        /// Check whether final direction in sector, which activate chair rotation
        /// </summary>
        /// <param name="yAxisRotation"></param>
        /// <returns></returns>
        private bool IsInBorder(float yAxisRotation,out int chairRotationSide)
        {
            yAxisRotation = (yAxisRotation + 360) % 360;
            if (CurrentChairDirection < ChairBorderLimit || CurrentChairDirection > 360 - ChairBorderLimit)
            {
                float rightLimit = (CurrentChairDirection + ChairBorderLimit) % 360;
                float leftLimit = (CurrentChairDirection - ChairBorderLimit + 360) % 360;
                float invDir = (CurrentChairDirection + 180) % 360;
                chairRotationSide = yAxisRotation > invDir ? -1 : 1;
                return (yAxisRotation > rightLimit &&
                    yAxisRotation < leftLimit);
            }
            else
            {
                float yNormalized = yAxisRotation - CurrentChairDirection;
                if (yNormalized < 0) yNormalized += 360;
                chairRotationSide = yNormalized > 180 ? -1 : 1;
                return !(yAxisRotation>CurrentChairDirection-ChairBorderLimit&&
                        yAxisRotation< CurrentChairDirection+ChairBorderLimit);
            }
        }
        private float LimitYAxisRotation(float yAxisRotation)
        {
            yAxisRotation = (yAxisRotation + 360) % 360;
            if (CurrentChairDirection< ChairFreeRadius || CurrentChairDirection > 360 - ChairFreeRadius)
            {
                float rightLimit = (CurrentChairDirection + ChairFreeRadius)%360;
                float leftLimit = (CurrentChairDirection - ChairFreeRadius + 360)%360;
                if (yAxisRotation > rightLimit &&
                    yAxisRotation < leftLimit)
                {
                    float invDir = (CurrentChairDirection + 180) % 360;
                    return yAxisRotation > invDir ? leftLimit : rightLimit;
                }
            }
            else
            {
                float rightLimit = CurrentChairDirection + ChairFreeRadius;
                float leftLimit = CurrentChairDirection - ChairFreeRadius;
                if(!(yAxisRotation>leftLimit&&
                    yAxisRotation < rightLimit))
                {
                    float yNormalized = yAxisRotation - CurrentChairDirection;
                    if (yNormalized < 0) yNormalized += 360;
                    return yNormalized > 180 ? leftLimit : rightLimit;
                }
            }
            return yAxisRotation;
        }
        public void SetRotation(Vector3 rotation)
        {
            int side;
            if (IsInBorder(rotation.y,out side))
            {
                float newY =LimitYAxisRotation(rotation.y);
                if (side != ChairRotationSide)
                    StartChairRotationEvent(side);
                ChairRotationSide = side;
                rotation = new Vector3(rotation.x, newY, rotation.z);
            }
            else if (ChairRotationSide != 0)
            {
                ChairRotationSide = 0;
                StopChairRotationEvent();
            }
            CurrentViewRotation = rotation;
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

        private void RotateChair()
        {
            if (ChairRotationSide != 0)
            {
                SetChairRotation(CurrentChairDirection + (ChairRotationSide * ChairDirectionChangeSpeed));
                if (!IsInBorder(CurrentViewRotation.y,out int side))
                {
                    ChairRotationSide = 0;
                    StopChairRotationEvent();
                }
                else if(side!=ChairRotationSide)
                {
                    ChairRotationSide = side;
                    StartChairRotationEvent(ChairRotationSide);
                }
            }
        }
        private void SetChairRotation(float rotation)
        {
            CurrentChairDirection = (rotation+360)%360;
            ChangeChairDirectionEvent(CurrentChairDirection);
        }
        private void OnEnable()
        {
            SetChairRotation(StartChairDirection);
        }
        private void LateUpdate()
        {
            RotateChair();
            ViewObject.eulerAngles = CurrentViewRotation+(CameraStagger!=null?CameraStagger.StaggerRotationOffset_:Vector3.zero);
            ChangeViewRotationEvent(CurrentViewRotation, -ViewObject.eulerAngles.y + 90);
            ChangeViewDirectionEvent(CurrentViewDirection_, CurrentHorizontalViewDirection_);
        }

        public Vector3 CurrentViewDirection_ => CurrentViewRotation.DirectionFromAngle3D();
        public Vector2 CurrentHorizontalViewDirection_ => (-ViewObject.eulerAngles.y + 90).DirectionOfAngle();

        void CameraBehaviourSelector.ITurnedOffCameraBehaviours.TurnOn() => enabled = true;
        void CameraBehaviourSelector.ITurnedOffCameraBehaviours.TurnOff() => enabled = false;
    }
}
