using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class FolderHitSoundPlayer : MonoBehaviour
    {
        [SerializeField] private float ImpulseMinMagnitude;
        [SerializeField] private GameObject SoundPrefab;
        [SerializeField] private AudioClip SoundClip;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.impulse.magnitude >= ImpulseMinMagnitude)
            {
                OneShotSoundCreator.PlaySound(SoundPrefab, SoundClip, transform.position, transform);
            }
        }
    }
}
