using AIAD.Player.COM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class Chair : MonoBehaviour
    {
        [SerializeField] private CameraBehaviour_Chair CameraBehaviour;
        [SerializeField] private ChairMovingModule MovingModule;

        private void StartMovingAction(Vector3 direction)
        {
            enabled = true;
            MovingModule.StartMovingEvent -= StartMovingAction;
        }
        private void StopMovingAction()
        {
            enabled = false;
        }
        private void Rotate(float angle)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
        }
        
        private void Deactivation()
        {
            if (MovingModule != null)
            {
                MovingModule.StartMovingEvent += StartMovingAction;
            }
            enabled = false;
        }
        private void Awake()
        {
            CameraBehaviour.ChangeChairDirectionEvent += Rotate;
            Deactivation();
        }
        private void FixedUpdate()
        {
            if (MovingModule.IsLocked_)
                Deactivation();
            else
            {
                transform.position = new Vector3
                    (MovingModule.CurrentMovableObjectPosition_.x,
                    transform.position.y,
                    MovingModule.CurrentMovableObjectPosition_.z);
            }
        }
    }
}
