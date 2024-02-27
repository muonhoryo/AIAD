
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace AIAD
{
    public static class AudioSettings
    {
        public static event Action<int> GlobalVolumeLevelChangedEvent = delegate { };
        public static event Action<int> SoundsVolumeLevelChangedEvent = delegate { };
        public static event Action<int> MusicVolumeLevelChangedEvent = delegate { };
        public static event Action<int> AmbientVolumeLevelChangedEvent = delegate { };

        public enum AudioVolumeType
        {
            Global,
            Sounds,
            Music,
            Ambient
        }

        private static AudioMixer MasterMixer;
        private static string GlobalVolumeLevelName;
        private static string SoundsVolumeLevelName;
        private static string MusicVolumeLevelName;
        private static string AmbientVolumeLevelName;

        public static void SetConsts(AudioMixer MasterMixer,string GlobalVolumeLevelName,
            string SoundsVolumeLevelName, string MusicVolumeLevelName, string AmbientVolumeLevelName)
        {
            AudioSettings.MasterMixer= MasterMixer;
            AudioSettings.GlobalVolumeLevelName= GlobalVolumeLevelName;
            AudioSettings.SoundsVolumeLevelName= SoundsVolumeLevelName;
            AudioSettings.MusicVolumeLevelName = MusicVolumeLevelName;
            AudioSettings.AmbientVolumeLevelName= AmbientVolumeLevelName;
            GlobalVolumeLevel_ = GlobalVolumeLevel_;
            SoundsVolumeLevel_ = SoundsVolumeLevel_;
            MusicVolumeLevel_ = MusicVolumeLevel_;
            AmbientVolumeLevel_ = AmbientVolumeLevel_;
        }

        private static void SetVolume(ref int volumeField,int volume,in string volumeName)
        {
            volumeField = Mathf.Clamp(volume, 0, 100);
            float volumeEffectValue = Mathf.Log10((float)volumeField / 100)*10;
            if (volumeEffectValue <= -23) volumeEffectValue = -80;
            MasterMixer.SetFloat(volumeName,  volumeEffectValue);
        }

        private static int GlobalVolumeLevel=100;
        private static int SoundsVolumeLevel = 100;
        private static int MusicVolumeLevel = 100;
        private static int AmbientVolumeLevel = 100;

        public static int GlobalVolumeLevel_ 
        {
            get => GlobalVolumeLevel;
            set
            {
                SetVolume(ref GlobalVolumeLevel, value, in GlobalVolumeLevelName);
                GlobalVolumeLevelChangedEvent(GlobalVolumeLevel);
            }
        }
        public static int SoundsVolumeLevel_
        {
            get => SoundsVolumeLevel;
            set
            {
                SetVolume(ref SoundsVolumeLevel, value, in SoundsVolumeLevelName);
                SoundsVolumeLevelChangedEvent(SoundsVolumeLevel);
            }
        }
        public static int MusicVolumeLevel_
        {
            get => MusicVolumeLevel;
            set
            {
                SetVolume(ref MusicVolumeLevel, value, in MusicVolumeLevelName);
                MusicVolumeLevelChangedEvent(MusicVolumeLevel);
            }
        }
        public static int AmbientVolumeLevel_
        {
            get => AmbientVolumeLevel;
            set
            {
                SetVolume(ref AmbientVolumeLevel, value, in AmbientVolumeLevelName);
                AmbientVolumeLevelChangedEvent(AmbientVolumeLevel);
            }
        }
    }
}
