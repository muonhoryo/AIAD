using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AIAD
{
    public sealed class SceneReloader : MonoBehaviour
    {
        public void ReloadScene()
        {
            ReturnToMainMenu();
        }
        public static void ReturnToMainMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
