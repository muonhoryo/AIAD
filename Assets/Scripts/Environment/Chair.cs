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
        }
        private void StopMovingAction()
        {
            enabled = false;
        }
        private void Rotate(float angle)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
        }
        private void Awake()
        {
            CameraBehaviour.ChangeChairDirectionEvent += Rotate;
            if (MovingModule != null)
            {
                MovingModule.StartMovingEvent += StartMovingAction;
                MovingModule.StopMovingEvent += StopMovingAction;
            }
            enabled = false;
        }
        private void FixedUpdate()
        {
            transform.position = new Vector3
                (MovingModule.CurrentMovableObjectPosition_.x,
                transform.position.y,
                MovingModule.CurrentMovableObjectPosition_.z);
        }
    }
}
