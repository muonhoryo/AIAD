
using AIAD.Exceptions;
using static AIAD.AudioSettings;
using UnityEngine;

namespace AIAD
{
    public sealed class AnimationAudioVolumeChanger : MonoBehaviour
    {
        public AudioVolumeType ChangedVolume;
        public int VolumeLevel;

        private void Update()
        {
            switch(ChangedVolume)
            {
                case AudioVolumeType.Global: GlobalVolumeLevel_ = VolumeLevel; break;
                case AudioVolumeType.Sounds: SoundsVolumeLevel_ = VolumeLevel; break;
                case AudioVolumeType.Music: MusicVolumeLevel_ = VolumeLevel; break;
                case AudioVolumeType.Ambient: AmbientVolumeLevel_ = VolumeLevel; break;

                default: throw new AIADException("Undefined volume type.", "AnimationAudioVolumeChanger.Update()");
            }
        }
    }
}
