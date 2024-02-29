using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class MusicChanger : MonoBehaviour
    {
        [SerializeField] private AudioSource MusicSource;

        private void Awake()
        {
            Registry.MusicChanger = this;
        }

        public void SetMusic(AudioClip musicClip)
        {
            float currentMscDur = MusicSource.clip.length;
            float currentMscPlayTime = MusicSource.time;
            MusicSource.clip = musicClip;
            MusicSource.time=currentMscPlayTime%Mathf.Min(currentMscDur,MusicSource.clip.length);
            MusicSource.Play();
        }
    }
}
