
using AIAD.Exceptions;
using System.Collections;
using UnityEngine;

namespace AIAD
{
    public sealed class OneShotSoundCaller : MonoBehaviour
    {
        [SerializeField] private GameObject SoundPrefab;
        public void PlaySound(AudioClip playedClip)
        {
            OneShotSoundCreator.PlaySound(SoundPrefab, playedClip, transform.position, transform);
        }
    }
}
