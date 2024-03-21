using AIAD.Exceptions;
using AIAD.Player.COM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Player.COM
{
    public class DroneMovDirCalcModule : MonoBehaviour, IMovDirCalcModule
    {
        [SerializeField] private MonoBehaviour CameraBehaviourComponent;

        private ICameraViewBehaviour CameraBehaviour;

        private AIADException MissingViewModuleException(string calledFuncName) =>
            new AIADMissingModuleException("ViewModule", $"MovDirFromViewDirModule.{calledFuncName}()");
        private void Awake()
        {
            CameraBehaviour = CameraBehaviourComponent as ICameraViewBehaviour;
            if (CameraBehaviour == null)
                throw MissingViewModuleException("Awake");
        }
        Vector3 IMovDirCalcModule.GetDirection(float HorizontalAxisValue, float VerticalAxisValue)
        {
            if (CameraBehaviour == null)
                throw MissingViewModuleException("IMovDirCalcModule.GetDirection()");
            if (HorizontalAxisValue != 0)
            {
                CameraBehaviour.Rotate(new Vector3(0, 0, HorizontalAxisValue*ExternalConsts.Consts_.DroneCameraSensitive_Z ));
            }
            return CameraBehaviour.CurrentViewDirection_ * VerticalAxisValue;
        }
    }
}
