
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD
{
    public sealed class LampEmergencyTurning : MonoBehaviour
    {
        [SerializeField] private EmergencyState EmergencyState;
        [SerializeField] private GameObject TurningOffObject;
        [SerializeField] private GameObject TurningOnObject;

        private void EmergencyAction()
        {
            TurningOffObject.SetActive(false);
            TurningOnObject.SetActive(true);
        }
        private void Awake()
        {
            if (EmergencyState == null)
                throw new AIADException("Missing EmergencyState.", "LampEmergencyTurning.Awake()");

            EmergencyState.OnEmergencyEvent += EmergencyAction;
        }
    }
}
