
using System;
using UnityEngine;

namespace AIAD
{
    public sealed class DebugConsoleController:MonoBehaviour
    {
        public event Action ConsoleControllerEnabledEvent = delegate { };
        public event Action ConsoleControllerDisabledEvent = delegate { };

        public bool IsActive_ { get; private set; } = false;

        private void ActivationAction()
        {
            IsActive_= true;
            Registry.PLayerController.enabled = false;
            ConsoleControllerEnabledEvent();
        }
        private void DeactivationAction()
        {
            IsActive_ = false;
            Registry.PLayerController.enabled = true;
            ConsoleControllerDisabledEvent();
        }

        private void Awake()
        {
            Registry.DebugConsoleController = this;
            DontDestroyOnLoad(gameObject);
        }

        private void InactiveUpdate()
        {
            if (Input.GetKeyUp(KeyCode.BackQuote))
            {
                ActivationAction();
            }
        }
        private void ActiveUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                DebugConsole.RunCommand();
                DeactivationAction();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                DebugConsole.RemoveLast();
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                DeactivationAction();
            }
            else
            {
                DebugConsole.AddKeyboardInput(Input.inputString);
            }
        }
        private void Update()
        {
            if (IsActive_)
                ActiveUpdate();
            else
                InactiveUpdate();
        }
    }
}
