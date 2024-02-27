
using UnityEngine;
using UnityEngine.UI;

namespace AIAD
{
    public sealed class DebugConsoleField:MonoBehaviour
    {
        [SerializeField] private Text TextComponent;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void ConsoleInputChangedAction(string newInput)
        {
            TextComponent.text = newInput;
        }
        private void ConsoleActivationAction()
        {
            enabled = true;
            DebugConsole.InputChangedEvent += ConsoleInputChangedAction;
            DebugConsole.ConsoleCommandExecutedEvent += ConsoleInputChangedAction;
            ConsoleInputChangedAction(DebugConsole.ConsoleInput);
        }
        private void ConsoleDeactivationAction()
        {
            enabled = false;
            DebugConsole.InputChangedEvent -= ConsoleInputChangedAction;
            DebugConsole.ConsoleCommandExecutedEvent-= ConsoleInputChangedAction;
        }
        private void Start()
        {
            Registry.DebugConsoleController.ConsoleControllerEnabledEvent += ConsoleActivationAction;
            Registry.DebugConsoleController.ConsoleControllerDisabledEvent += ConsoleDeactivationAction;
            ConsoleDeactivationAction();
        }
    }
}
