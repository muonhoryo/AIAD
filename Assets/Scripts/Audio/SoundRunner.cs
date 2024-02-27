
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD
{
    public sealed class SoundRunner : MonoBehaviour
    {
        [SerializeField] private AudioSource SoundSource;

        private void Awake()
        {
            if (SoundSource == null)
                throw new AIADException("Missing PlayedSound.", "CameraSoundRunner.Awake()");
        }

        public void PlaySound(AudioClip playedSound)
        {
            if (playedSound == null)
                throw new AIADException("Missing playedSound.", "CameraSoundRunner.PlaySound()");

            SoundSource.clip = playedSound;
            SoundSource.Play();
        }
    }
}
