
using AIAD.Exceptions;
using AIAD.SL;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace AIAD
{
    public sealed class Initialization : MonoBehaviour
    {
        [SerializeField] private AudioMixer MasterMixer;
        [SerializeField] private string GlobalVolumeLevelName;
        [SerializeField] private string SoundsVolumeLevelName;
        [SerializeField] private string MusicVolumeLevelName;
        [SerializeField] private string AmbientVolumeLevelName;

        [SerializeField] private string SoundsTestVolumeLevelName;
        [SerializeField] private string MusicTestVolumeLevelName;
        [SerializeField] private string AmbientTestVolumeLevelName;

        [SerializeField] private string SLScriptsSerializationPath;
        [SerializeField] private string SLScriptsFilesType;
        [SerializeField] private string SoundsPath;
        [SerializeField] private string MusicPath;
        [SerializeField] private string MainSceneName;

        private void Awake()
        {
            string ExcSrc = "Initialization.Awake()";

            if (string.IsNullOrEmpty(SLScriptsSerializationPath))
                throw new AIADException("SLScriptsSerializationPath cannot be null or empty.", ExcSrc);
            if (string.IsNullOrEmpty(SLScriptsFilesType))
                throw new AIADException("SLScriptsFilesType cannot be null or empty.", ExcSrc);
            if (string.IsNullOrWhiteSpace(SoundsPath))
                throw new AIADException("SoundsPath cannot be null or empty.", ExcSrc);
            if (string.IsNullOrWhiteSpace(MusicPath))
                throw new AIADException("MusicPath cannot be null or empty.", ExcSrc);
            if (string.IsNullOrEmpty(GlobalVolumeLevelName))
                throw new AIADException("GlobalVolumeLevelName cannot be null or empty.", ExcSrc);
            if (string.IsNullOrEmpty(SoundsVolumeLevelName))
                throw new AIADException("SoundsVolumeLevelName cannot be null or empty.", ExcSrc);
            if (string.IsNullOrEmpty(MusicVolumeLevelName))
                throw new AIADException("MusicVolumeLevelName cannot be null or empty.", ExcSrc);
            if (string.IsNullOrEmpty(AmbientVolumeLevelName))
                throw new AIADException("AmbientVolumeLevelName cannot be null or empty.", ExcSrc);
            if (string.IsNullOrEmpty(SoundsTestVolumeLevelName))
                throw new AIADException("SoundsTestVolumeLevelName cannot be null or empty.", ExcSrc);
            if (string.IsNullOrEmpty(MusicTestVolumeLevelName))
                throw new AIADException("MusicTestVolumeLevelName cannot be null or empty.", ExcSrc);
            if (string.IsNullOrEmpty(AmbientTestVolumeLevelName))
                throw new AIADException("AmbientTestVolumeLevelName cannot be null or empty.", ExcSrc);
            if (MasterMixer == null)
                throw new AIADException("Missing MasterMixer.", ExcSrc);

            SL_ScriptManager.ScriptsSerializationPath = SLScriptsSerializationPath;
            SL_ScriptManager.ScriptsFilesType = SLScriptsFilesType;

            AudioManager.MusicPath = MusicPath;
            AudioManager.SoundsPath= SoundsPath;

            AudioSettings.SetConsts(MasterMixer, GlobalVolumeLevelName, SoundsVolumeLevelName, MusicVolumeLevelName, AmbientVolumeLevelName,
                SoundsTestVolumeLevelName,MusicTestVolumeLevelName,AmbientTestVolumeLevelName);

            SceneManager.LoadScene(MainSceneName, LoadSceneMode.Single);
        }
    }
}
