
using AIAD.Player.COM;
using System;
using System.IO;
using UnityEngine;

namespace AIAD
{
    public sealed class PlayerHPSystemsInit : MonoBehaviour
    {
        public static PlayerHPConsts Consts { get; private set; }

        [SerializeField] private string SerializationPath;
#if UNITY_EDITOR
        [SerializeField][Range(1, 1000000)] private int MaxHP;
        [SerializeField][Range(1, 1000000)] private int LowHP;
        [SerializeField][Range(0, 10)] private float DecreaseTimeInterval;
        [SerializeField][Range(0, HitPointDecreaser.MaxDecreasedPointCount)] private int DecreaseCount;
        [SerializeField][Range(0,10000)] private float DecBooster_MaxRemTime;
#endif
        [Serializable]
        public struct PlayerHPConsts 
        {
            public int MaxHP;
            public int LowHP;
            public float DecreaseTimeInterval;
            public int DecreaseCount;
            public float DecBooster_MaxRemTime;

            public PlayerHPConsts(int maxHP, int lowHP, float decreaseTimeInterval, int decreaseCount, float decBooster_MaxRemTime)
            {
                MaxHP = maxHP;
                LowHP = lowHP;
                DecreaseTimeInterval = decreaseTimeInterval;
                DecreaseCount = decreaseCount;
                DecBooster_MaxRemTime = decBooster_MaxRemTime;
            }
        }

        private void Awake()
        {
            using (StreamReader reader = new StreamReader(SerializationPath))
            {
                Consts = JsonUtility.FromJson<PlayerHPConsts>(reader.ReadToEnd());
            }
        }

#if UNITY_EDITOR
        [ContextMenu("SerializeConsts")]
        public void Serialize()
        {
            using(StreamWriter writer=new StreamWriter(SerializationPath,false))
            {
                var consts = new PlayerHPConsts(MaxHP, LowHP, DecreaseTimeInterval, DecreaseCount, DecBooster_MaxRemTime);
                writer.Write(JsonUtility.ToJson(consts, true));
            }
        }
#endif
    }
}
