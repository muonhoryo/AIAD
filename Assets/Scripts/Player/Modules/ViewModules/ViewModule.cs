using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIAD;
using MuonhoryoLibrary.Unity;
using AIAD.Exceptions;

namespace AIAD.Player.COM
{
    public sealed class ViewModule : MonoBehaviour, IViewDirectionModule
    {
        public event Action<Vector3,Vector2> ChangeViewDirectionEvent = delegate { };

        [SerializeField] private MonoBehaviour CameraBehaviourComponent;
        private ICameraViewBehaviour CameraViewBehaviour;

        [SerializeField] private float XSensitive;
        [SerializeField] private float YSensitive;

        public void ChangeViewDirection(float XMoving, float YMoving)
        {
            float YStep = XMoving * XSensitive; //Rotate around YAxis, horizontal rotation
            float XStep = -YMoving * YSensitive; //Rotate around XAxis, vertical rotation
            CameraViewBehaviour.Rotate(new Vector2(XStep, YStep));

            ChangeViewDirectionEvent(CurrentViewDirection_, CurrentHorizontalViewDirection_);
        }

        private void Awake()
        {
            CameraViewBehaviour = CameraBehaviourComponent as ICameraViewBehaviour;
            if (CameraViewBehaviour == null)
                throw new AIADException("CameraViewBehaviour cannot be null. ", "StandingViewChangingModule.Awake()");
            CameraViewBehaviour.ChangeViewDirectionEvent += (rot, dir) => ChangeViewDirectionEvent(rot,dir);
        }


        void IViewDirectionModule.ChangeViewDirection(float XMoving, float YMoving) => ChangeViewDirection(XMoving, YMoving);
        public Vector3 CurrentViewDirection_ => CameraViewBehaviour.CurrentViewDirection_;
        public Vector2 CurrentHorizontalViewDirection_ => CameraViewBehaviour.CurrentHorizontalViewDirection_;
    }
}
