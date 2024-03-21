
using AIAD.Player.COM;
using System;
using System.IO;
using UnityEngine;

namespace AIAD
{
    public sealed class ExternalConsts : MonoBehaviour
    {
        public static Consts Consts_ { get; private set; }

        [SerializeField] private string SerializationPath;
#if UNITY_EDITOR
        [SerializeField][Range(1, 1000000)] private int MaxHP;
        [SerializeField][Range(1, 1000000)] private int LowHP;
        [SerializeField][Range(0, 10)] private float DecreaseTimeInterval;
        [SerializeField][Range(0, HitPointDecreaser.MaxDecreasedPointCount)] private int DecreaseCount;
        [SerializeField][Range(0,10000)] private float DecBooster_MaxRemTime;

        [SerializeField][Range(0, 100)] private float StandMovCameraSensitive_X;
        [SerializeField][Range(0, 100)] private float StandMovCameraSensitive_Y;
        [SerializeField][Range(0, 100)] private float DroneCameraSensitive_Z;

        [SerializeField][Range(0, 1000)] private float PlayerStandMovingSpeed;
        [SerializeField][Range(0, 1000)] private float DroneMovingSpeed;
        [SerializeField][Range(0, 1000)] private float PlayerChairMovingSpeed;
#endif
        [Serializable]
        public struct Consts 
        {
            public int MaxHP;
            public int LowHP;
            public float DecreaseTimeInterval;
            public int DecreaseCount;
            public float DecBooster_MaxRemTime;

            public float StandMovCameraSensitive_X;
            public float StandMovCameraSensitive_Y;
            public float DroneCameraSensitive_Z;

            public float PlayerStandMovingSpeed;
            public float DroneMovingSpeed;
            public float PlayerChairMovingSpeed;

            public Consts(int maxHP,
                          int lowHP,
                          float decreaseTimeInterval,
                          int decreaseCount,
                          float decBooster_MaxRemTime,
                          float standMovCameraSensitive_X,
                          float standMovCameraSensitive_Y,
                          float droneCameraSensitive_Z,
                          float playerStandMovingSpeed,
                          float droneMovingSpeed,
                          float playerChairMovingSpeed)
            {
                MaxHP = maxHP;
                LowHP = lowHP;
                DecreaseTimeInterval = decreaseTimeInterval;
                DecreaseCount = decreaseCount;
                DecBooster_MaxRemTime = decBooster_MaxRemTime;
                StandMovCameraSensitive_X = standMovCameraSensitive_X;
                StandMovCameraSensitive_Y = standMovCameraSensitive_Y;
                DroneCameraSensitive_Z = droneCameraSensitive_Z;
                PlayerStandMovingSpeed = playerStandMovingSpeed;
                DroneMovingSpeed = droneMovingSpeed;
                PlayerChairMovingSpeed = playerChairMovingSpeed;
            }
        }

        private void Awake()
        {
            using (StreamReader reader = new StreamReader(SerializationPath))
            {
                Consts_ = JsonUtility.FromJson<Consts>(reader.ReadToEnd());
            }
        }

#if UNITY_EDITOR
        [ContextMenu("SerializeConsts")]
        public void Serialize()
        {
            using(StreamWriter writer=new StreamWriter(SerializationPath,false))
            {
                var consts = new Consts(MaxHP, LowHP, DecreaseTimeInterval, DecreaseCount, DecBooster_MaxRemTime,
                    StandMovCameraSensitive_X,StandMovCameraSensitive_Y,DroneCameraSensitive_Z,
                    PlayerStandMovingSpeed,DroneMovingSpeed,PlayerChairMovingSpeed);

                writer.Write(JsonUtility.ToJson(consts, true));
            }
        }
#endif
    }
}
