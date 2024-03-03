
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD
{
    public sealed class MusicRemainder : MonoBehaviour
    {
        [SerializeField] private float FadingSpeed;
        [SerializeField] private AudioSource MusicSource;

        private void Awake()
        {
            string ExcSrc = "MusicRemainder.Awake()";

            if (FadingSpeed <= 0)
                throw new AIADException("FadingSpeed must be greater than zero.", ExcSrc);
            if (MusicSource == null)
                throw new AIADException("Missing MusicSource.", ExcSrc);
        }

        public void Initialize(float startTime,AudioClip clip)
        {
            if (clip == null)
                throw new AIADException("Missing clip", "MusicRemainder.Initialize()");
            MusicSource.clip = clip;
            MusicSource.time =startTime;
            MusicSource.Play();
        }

        private void FixedUpdate()
        {
            if(MusicSource.volume > 0)
            {
                MusicSource.volume -= FadingSpeed;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
    }
}
