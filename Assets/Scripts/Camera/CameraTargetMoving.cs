using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class CameraTargetMoving : MonoBehaviour
    {
        [SerializeField] private Transform Camera;
        [SerializeField] private Transform Target;

        [SerializeField] private Vector3 Offset;

        private void LateUpdate()
        {
            if (Target == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Target is missing");
#endif
                return;
            }
            Camera.position = Target.position+Offset;
        }
        public void ChangeTarget(Transform newTarget)
        {
            Target=newTarget;
        }
        public void SetFollowingOffset(Vector3 offset)
        {
            Offset = offset;
            Camera.position = Target.position + Offset;
        }

        private void Awake()
        {
            Registry.MainCameraMovingBehaviour = this;
        }
    }
}
