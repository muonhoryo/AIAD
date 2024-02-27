

using AIAD.SL;
using System;
using UnityEngine;

namespace AIAD
{
    public static class DebugConsole
    {
        public static event Action<string> InputChangedEvent = delegate { };
        public static event Action<string> ConsoleCommandExecutedEvent = delegate { };

        public static string ConsoleInput { get; private set; } = "";

        public static void AddKeyboardInput(string input)
        {
            ConsoleInput = ConsoleInput + input;
            InputChangedEvent(ConsoleInput);
        }
        public static void RemoveLast()
        {
            if (ConsoleInput.Length > 0)
            {
                int newL = ConsoleInput.Length - 1;
                ConsoleInput = ConsoleInput.Substring(0, newL);
                InputChangedEvent(ConsoleInput);
            }
        }
        public static void RunCommand()
        {
            try
            {
                SL_Executor.ExecuteSingleCommand(ConsoleInput);
                ConsoleCommandExecutedEvent(ConsoleInput);
            }
            catch
            {
#if UNITY_EDITOR
                Debug.Log("Unknown command");
#endif
            }
            ConsoleInput = "";
        }
    }
}
