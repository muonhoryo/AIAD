using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class MusicChanger : MonoBehaviour
    {
        [SerializeField] private AudioSource MusicSource;
        [SerializeField] private GameObject MusicRemainderPrefab;
        [SerializeField] private float FadingSpeed;

        private void Awake()
        {
            Registry.MusicChanger = this;
        }

        public void SetMusic(AudioClip musicClip)
        {
            float currentMscDur = MusicSource.clip.length;
            float currentMscPlayTime = MusicSource.time;
            MusicRemainder remScript =Instantiate(MusicRemainderPrefab, gameObject.transform).GetComponent<MusicRemainder>();
            remScript.Initialize(currentMscPlayTime, MusicSource.clip);
            MusicSource.clip = musicClip;
            MusicSource.time=currentMscPlayTime%Mathf.Min(currentMscDur,MusicSource.clip.length);
            MusicSource.Play();
            StopAllCoroutines();
            float targetVolume=MusicSource.volume;
            MusicSource.volume = 0;
            StartCoroutine(VolumeUnfading(targetVolume));
        }

        private IEnumerator VolumeUnfading(float targetVolume)
        {
            while (true)
            {
                MusicSource.volume += FadingSpeed;
                if (MusicSource.volume >= targetVolume)
                {
                    MusicSource.volume = targetVolume;
                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
