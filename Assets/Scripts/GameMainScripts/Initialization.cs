
using AIAD.Exceptions;
using AIAD.SL;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AIAD
{
    public sealed class Initialization : MonoBehaviour
    {
        [SerializeField] private string SLScriptsSerializationPath;
        [SerializeField] private string SLScriptsFilesType;
        [SerializeField] private string MainSceneName;

        private void Awake()
        {
            if (string.IsNullOrEmpty(SLScriptsSerializationPath))
                throw new AIADException("SLScriptsSerializationPath cannot be null or empty.", "Initialization.Awake()");
            if (string.IsNullOrEmpty(SLScriptsFilesType))
                throw new AIADException("SLScriptsFilesType cannot be null or empty.", "Initialization.Awake()");

            SL_ScriptManager.ScriptsSerializationPath = SLScriptsSerializationPath;
            SL_ScriptManager.ScriptsFilesType = SLScriptsFilesType;

            SceneManager.LoadScene(MainSceneName, LoadSceneMode.Single);
        }
    }
}
