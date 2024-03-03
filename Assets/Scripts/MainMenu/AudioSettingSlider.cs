using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AIAD
{
    public sealed class AudioSettingSlider : MonoBehaviour
    {
        [SerializeField] private Slider Owner;
        [SerializeField] private AudioSettings.AudioVolumeType AudioType;

        public void ChangeAudioVolume()
        {
            AudioSettings.SetAudioVolume(AudioType, (int)Owner.value);
        }
    }
}
