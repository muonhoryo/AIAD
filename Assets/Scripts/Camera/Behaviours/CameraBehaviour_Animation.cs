using AIAD.SL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuonhoryoLibrary.Unity;
using Unity.VisualScripting;

namespace AIAD
{
    public sealed class CameraBehaviour_Animation : MonoBehaviour,CameraBehaviourSelector.ITurnedOffCameraBehaviours
    {
        public event Action<Vector3, float> ChangeViewRotationEvent = delegate { };
        public event Action<Vector3, Vector2> ChangeViewDirectionEvent = delegate { };
        public event Action AnimationDoneEvent = delegate { };

        [SerializeField] private Transform ViewObject;
        [SerializeField] private int StepsToMergeWithAnimation;

        public Vector3 Rotation=Vector3.zero;
        public Vector3 Position = Vector3.zero;
        private Vector3 LocalRotationCenter = Vector3.zero;
        private Vector3 LocalPositionCenter = Vector3.zero;

        private bool IsAnimatePosition = true;
        private bool IsAnimateRotation = true;

        private bool IsGlobalAnimPosition_X = true;
        private bool IsGlobalAnimPosition_Y = true;
        private bool IsGlobalAnimPosition_Z = true;
        private bool IsGlobalAnimRotation_X = true;
        private bool IsGlobalAnimRotation_Y = true;
        private bool IsGlobalAnimRotation_Z = true;

        private int RemainingSteps=0;

        private void LateUpdate()
        {
            Vector3 rotation = Vector3.zero;
            Vector3 position=Vector3.zero;

            if (IsAnimateRotation)
            {
                rotation= new Vector3(
                        IsGlobalAnimRotation_X ? Rotation.x : Rotation.x + LocalRotationCenter.x,
                        IsGlobalAnimRotation_Y ? Rotation.y : Rotation.y + LocalRotationCenter.y,
                        IsGlobalAnimRotation_Z ? Rotation.z : Rotation.z + LocalRotationCenter.z);
            }

            if (IsAnimatePosition)
            {
                position = new Vector3(
                        IsGlobalAnimPosition_X ? Position.x : Position.x + LocalPositionCenter.x,
                        IsGlobalAnimPosition_Y ? Position.y : Position.y + LocalPositionCenter.y,
                        IsGlobalAnimPosition_Z ? Position.z : Position.z + LocalPositionCenter.z);
            }
            
           

            if (RemainingSteps > 0)
            {
                int divider = RemainingSteps + 1;
                void RotateToTarget(Vector3 targetRotation)
                {
                    float GetRotationStep(float target, float current, int div)
                    {
                        float step;
                        if (current > target)
                        {
                            float diff = current - target;
                            step = diff > 180 ? 360 - diff : -diff;
                        }
                        else
                        {
                            float diff = target - current;
                            step = diff > 180 ? diff - 360 : diff;
                        }
                        return step / div;
                    }
                    float x, y, z;
                    x = GetRotationStep(targetRotation.x, ViewObject.eulerAngles.x, divider);
                    y = GetRotationStep(targetRotation.y, ViewObject.eulerAngles.y, divider);
                    z = GetRotationStep(targetRotation.z, ViewObject.eulerAngles.z, divider);
                    SetRotation(ViewObject.eulerAngles + new Vector3(x, y, z));
                }
                void MoveToTarget(Vector3 targetPosition)
                {
                    Vector3 newPosition = ViewObject.position + ((targetPosition - ViewObject.position) / divider);
                    ViewObject.position = newPosition;
                }

                if (IsAnimateRotation)
                    RotateToTarget(rotation);
                if (IsAnimatePosition)
                    MoveToTarget(position);
                RemainingSteps--;
            }
            else
            {
                if (IsAnimateRotation)
                    SetRotation(rotation);
                if (IsAnimatePosition)
                    ViewObject.position = position;
            }
        }

        public void AnimationDone(string SLScriptIDs)
        {
            AnimationDoneEvent();
            SLScriptIDs.RunSLScripts();
        }

        public void RunSLScripts(string SLScriptIDs)
        {
            SLScriptIDs.RunSLScripts();
        }

        public void SetCentersAsCurrent()
        {
            LocalRotationCenter = ViewObject.eulerAngles;
            LocalPositionCenter = ViewObject.position;
        }
        public void SetAnimationParams(bool IsAnimatePosition=true,bool IsAnimateRotation=true,
            bool IsGlobalAnimPosition_X=true,bool IsGlobalAnimPosition_Y=true,bool IsGlobalAnimPosition_Z=true,
            bool IsGlobalAnimRotation_X=true,bool IsGlobalAnimRotation_Y=true,bool IsGlobalAnimRotation_Z = true)
        {
            this.IsAnimatePosition= IsAnimatePosition;
            this.IsAnimateRotation= IsAnimateRotation;
            this.IsGlobalAnimPosition_X= IsGlobalAnimPosition_X;
            this.IsGlobalAnimPosition_Y= IsGlobalAnimPosition_Y;
            this.IsGlobalAnimPosition_Z = IsGlobalAnimPosition_Z;
            this.IsGlobalAnimRotation_X = IsGlobalAnimRotation_X;
            this.IsGlobalAnimRotation_Y = IsGlobalAnimRotation_Y;
            this.IsGlobalAnimRotation_Z = IsGlobalAnimRotation_Z;
        }

        void ICameraViewBehaviour.Rotate(Vector3 rotation) { }
        public void SetRotation(Vector3 rotation)
        {
            ViewObject.localEulerAngles = rotation;
            ChangeViewRotationEvent(ViewObject.eulerAngles, ViewObject.eulerAngles.y);
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
        void CameraBehaviourSelector.ITurnedOffCameraBehaviours.TurnOn() 
        {
            RemainingSteps = StepsToMergeWithAnimation;
            enabled = true;
        }
        void CameraBehaviourSelector.ITurnedOffCameraBehaviours.TurnOff() => enabled = false;
        public Vector3 CurrentViewDirection_ => ViewObject.eulerAngles.DirectionFromAngle3D();
        public Vector2 CurrentHorizontalViewDirection_=>(-ViewObject.eulerAngles.y + 90).DirectionOfAngle();
    }
}
