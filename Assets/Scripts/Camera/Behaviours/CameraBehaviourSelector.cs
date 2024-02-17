using System;
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD
{
    public sealed class CameraBehaviourSelector : MonoBehaviour,ICameraViewBehaviour
    {
        public interface ITurnedOffCameraBehaviours:ICameraViewBehaviour
        {
            public void TurnOff();
            public void TurnOn();
        }
        public event Action<ICameraViewBehaviour> SelectBehaviourEvent = delegate { };
        public event Action<Vector3,float> ChangeViewRotationEvent = delegate { };
        public event Action<Vector3, Vector2> ChangeViewDirectionEvent = delegate { };

        [SerializeField] private MonoBehaviour[] CameraBehavioursComponents;

        private ICameraViewBehaviour[] CameraBehaviours;
        private ICameraViewBehaviour CurrentBehaviour;

        private void Awake()
        {
            if(CameraBehavioursComponents != null && CameraBehavioursComponents.Length > 0)
            {
                CameraBehaviours=new ICameraViewBehaviour[CameraBehavioursComponents.Length];
                for(int i = 0; i < CameraBehavioursComponents.Length; i++)
                {
                    CameraBehaviours[i] = CameraBehavioursComponents[i] as ICameraViewBehaviour;
                    if (CameraBehaviours[i] == null)
                        throw new AIADException($"Cant parse component by index {i} in ICameraViewBehaviour.", "CameraBehaviourSelector.Awake()");
                    ITurnedOffCameraBehaviours parsedBeh = CameraBehaviours[i] as ITurnedOffCameraBehaviours;
                    if (parsedBeh != null)
                        parsedBeh.TurnOff();
                }
            }
            SelectBehaviour(0);
        }

        private void ChangeViewRotationAction(Vector3 i,float j)
        {
            ChangeViewRotationEvent(i,j);
        }
        private void ChangeViewDirectionAction(Vector3 i,Vector2 j)
        {
            ChangeViewDirectionEvent(i, j);
        }
        public void SelectBehaviour(int index)
        {
            if (CurrentBehaviour != null)
            {
                CurrentBehaviour.ChangeViewRotationEvent -= ChangeViewRotationAction;
                CurrentBehaviour.ChangeViewDirectionEvent -= ChangeViewDirectionAction;
                ITurnedOffCameraBehaviours parsedBeh = CurrentBehaviour as ITurnedOffCameraBehaviours;
                if (parsedBeh != null)
                    parsedBeh.TurnOff();
            }
            CurrentBehaviour = CameraBehaviours[index];
            if (CurrentBehaviour != null)
            {
                CurrentBehaviour.ChangeViewRotationEvent += ChangeViewRotationAction;
                CurrentBehaviour.ChangeViewDirectionEvent += ChangeViewDirectionAction;
                ITurnedOffCameraBehaviours parsedBeh = CurrentBehaviour as ITurnedOffCameraBehaviours;
                if (parsedBeh != null)
                    parsedBeh.TurnOn();
            }
            SelectBehaviourEvent(CurrentBehaviour);
        }

        Vector3 ICameraViewBehaviour.CurrentViewDirection_ => CurrentBehaviour != null ? CurrentBehaviour.CurrentViewDirection_ : Vector3.zero;
        Vector2 ICameraViewBehaviour.CurrentHorizontalViewDirection_ => 
            CurrentBehaviour != null ? CurrentBehaviour.CurrentHorizontalViewDirection_ : Vector2.zero;
        void ICameraViewBehaviour.Rotate(Vector3 rotation)
        {
            if (CurrentBehaviour != null)
                CurrentBehaviour.Rotate(rotation);
        }
        void ICameraViewBehaviour.SetRotation(Vector3 rotation)
        {
            if(CurrentBehaviour!=null)
                CurrentBehaviour.SetRotation(rotation);
        }
        void ICameraViewBehaviour.SetXRotation(float xRotation) => CurrentBehaviour?.SetXRotation(xRotation);
        void ICameraViewBehaviour.SetYRotation(float yRotation) => CurrentBehaviour?.SetYRotation(yRotation);
        void ICameraViewBehaviour.SetZRotation(float zRotation) => CurrentBehaviour?.SetZRotation(zRotation);
    }
}
