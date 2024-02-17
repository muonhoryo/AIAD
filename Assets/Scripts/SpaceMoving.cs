using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class SpaceMoving : MonoBehaviour
    {
        [Serializable]
        public sealed class MovingTextureCurrentState
        {
            [Serializable]
            public struct MovingTextureInfo
            {
                public MovingTextureInfo(Material Material, Vector2 MovingSpeed, string TextureName)
                {
                    this.Material = Material;
                    this.MovingSpeed = MovingSpeed;
                    this.TextureName = TextureName;
                }
                public Material Material;
                public Vector2 MovingSpeed;
                public string TextureName;
            }
            private MovingTextureCurrentState() { }
            public MovingTextureCurrentState(MovingTextureInfo Info)
            {
                this.Info = Info;
            }

            public void FixedUpdate()
            {
                CurrentAngle = (CurrentAngle +Info.MovingSpeed.x) % 360;
                Offset = new Vector2(Mathf.Cos(CurrentAngle * Mathf.Deg2Rad), Offset.y += Info.MovingSpeed.y);
            }
            public void SubscribeOnUpdateEvent(Action<Action> subscribeAction)
            {
                subscribeAction(() => { Info.Material.SetTextureOffset(Info.TextureName, Offset); });
            }
            [SerializeField]
            private MovingTextureInfo Info;
            private float CurrentAngle=0;
            private Vector2 Offset=Vector2.zero;
        }
        private event Action UpdateEvent;

        [SerializeField]
        private MovingTextureCurrentState[] MaterialsStates;
        private void FixedUpdate()
        {
            foreach (var state in MaterialsStates)
                state.FixedUpdate();
        }
        private void Update()
        {
            UpdateEvent?.Invoke();
        }
        private void Awake()
        {
            foreach (var state in MaterialsStates)
                state.SubscribeOnUpdateEvent((action) => UpdateEvent += action);
        }
    }
}
