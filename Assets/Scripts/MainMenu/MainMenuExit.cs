using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class MainMenuExit : MonoBehaviour
    {
        [SerializeField] private string ExitButtonName;

        private void Update()
        {
            if (Input.GetButtonDown(ExitButtonName))
            {
                SceneReloader.ReturnToMainMenu();
            }
        }
    }
}
