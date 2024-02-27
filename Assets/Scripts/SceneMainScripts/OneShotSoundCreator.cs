using AIAD.Exceptions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class OneShotSoundCreator : MonoBehaviour
    {
        private static OneShotSoundCreator singltone;

        [SerializeField] private GameObject SoundPrefab;

        private void Awake()
        {
            singltone = this;
            if (SoundPrefab == null)
                throw new AIADException("Missing SoundPrefab.", "OneShotSoundCaller.Awake()");
        }


        private static IEnumerator DestroyDelay(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(obj);
        }
        public static void PlaySound(GameObject soundPrefab,AudioClip playedClip,Vector3 position,Transform owner)
        {
            string ExcSrc = "OneShotSoundCaller.PlaySound()";

            if (playedClip == null)
                throw new AIADException("Missing playedClip.", ExcSrc);

            AudioSource src = Instantiate(soundPrefab, owner).GetComponent<AudioSource>();
            src.transform.position = position;

            if (src == null)
                throw new AIADException("Missing AudioSource.", ExcSrc);

            src.clip = playedClip;
            src.Play();
            singltone.StartCoroutine(DestroyDelay(src.gameObject, playedClip.length));
        }
        public static void PlaySound(AudioClip playedClip,Vector3 position,Transform owner)=>
            PlaySound(singltone.SoundPrefab,playedClip,position,owner);
    }
}
