
using AIAD.Exceptions;
using System.Collections;
using UnityEngine;

namespace AIAD
{
    public sealed class ChairRotationSoundPlayer : MonoBehaviour
    {
        [SerializeField] private CameraBehaviour_Chair Behaviour;
        [SerializeField] private AudioClip PlayedSound;
        [SerializeField] private GameObject SoundPrefab;
        [SerializeField] private float SoundPlayingDelay;

        private bool CanPlaySound = true;

        private IEnumerator SoundPlayDelay()
        {
            CanPlaySound = false;
            yield return new WaitForSeconds(SoundPlayingDelay);
            CanPlaySound = true;
        }
        private void StartRotationAction(int i)
        {
            if (CanPlaySound)
            {
                OneShotSoundCreator.PlaySound(SoundPrefab, PlayedSound, transform.position, transform);
                StartCoroutine(SoundPlayDelay());
            }
        }
        private void Awake()
        {
            string ExcSrc = "ChairRotationSoundPlayer.Awake()";

            if (Behaviour == null)
                throw new AIADException("Missing CameraBehaviour.", ExcSrc);
            if (PlayedSound == null)
                throw new AIADException("Missing PlayedSound.", ExcSrc);
            if (SoundPrefab == null)
                throw new AIADException("Missing SoundPrefab.", ExcSrc);

            Behaviour.StartChairRotationEvent += StartRotationAction;
        }
    }
}
