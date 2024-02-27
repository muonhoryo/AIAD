using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIAD.Exceptions;
using System;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using System.IO;

namespace AIAD.SL
{
    public static class SL_Executor
    {
        public static SL_ExecutorComponent ExecutorComponent;

        public static void ExecuteCommandsList(string[] commands)
        {
            int delayCommIndex;
            for (delayCommIndex = 0; delayCommIndex < commands.Length; delayCommIndex++)
            {
                int spaceIndex = commands[delayCommIndex].IndexOf(' ');
                if (spaceIndex == -1)
                    continue;
                string commandIdentifier = commands[delayCommIndex].Substring(0, spaceIndex);
                if (commandIdentifier == "Delay")
                {
                    float time;
                    {
                        //Parse time value
                        int lengthOfSerializedTime = commands[delayCommIndex].IndexOf(' ', spaceIndex + 1);
                        if (lengthOfSerializedTime == -1)
                            lengthOfSerializedTime = commands[delayCommIndex].Length - spaceIndex - 1;
                        else
                            lengthOfSerializedTime = lengthOfSerializedTime - spaceIndex - 1;

                        string serializedTimeValue = commands[delayCommIndex].Substring(spaceIndex + 1, lengthOfSerializedTime);

                        if (!float.TryParse(serializedTimeValue, out time))
                            throw new AIADException("Cant parse time value.", "SL_Execurot.ExecuteCommands()");
                    }
                    string[] delayedCommands = commands.Skip(delayCommIndex + 1).Take(commands.Length - delayCommIndex - 1).ToArray();
                    ExecutorComponent.StartCoroutine(DelayedCommandsExecution(delayedCommands, time));
                    break;
                }
            }
            for (int i = 0; i < delayCommIndex; i++)
            {
                ExecuteSingleCommand(commands[i]);
            }
        }
        private static IEnumerator DelayedCommandsExecution(string[] commands, float time)
        {
            yield return new WaitForSeconds(time);
            ExecuteCommandsList(commands);
        }

        /// <summary>
        /// If true command dont end script execution.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="AIADException"></exception>
        public static void ExecuteSingleCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new AIADException("Command cannot be null or empty.", "SL_Executor.ReadCommand()");
            }
            ExecuteCommandBySyntax(command.Split(' '));
        }
        private static void ExecuteCommandBySyntax(string[] syntax)
        {
            AIADException MissingControlLocker()
            {
                return new AIADException("Missing ControlLocker.", "SL_Executor.ExecuteCommand()");
            }


            switch (syntax[0])
            {
                //Lock player controller and other control scripts
                case "LockCtrl":
                    if (Registry.ControlLocker == null)
                        throw MissingControlLocker();
                    Registry.ControlLocker.LockControl();
                    break;

                //Unlock player controller and other control scripts
                case "UnlockCtrl":
                    if (Registry.ControlLocker == null)
                        throw MissingControlLocker();
                    Registry.ControlLocker.UnlockControl();
                    break;

                //Set camera view direction on selected value (format: x y z)/(format: x y)
                case "SetView":
                    Command_SetView(syntax);
                    break;

                //Turn off/on interaction module on object by ID (format: id state)
                case "TurnInteraction":
                    Command_TurnInteraction(syntax);
                    break;

                //Destroy whole object by ID
                case "Destroy":
                    Command_Destroy(syntax);
                    break;

                //Send message to object by ID to run command
                case "Message":
                    Command_Message(syntax);
                    break;

                //Set animator trigger on object by ID
                case "SetAnimTrigger":
                    Command_SetAnimTrigger(syntax);
                    break;

                //Add force to object by ID (format: id x y z)
                case "AddForce":
                    Command_AddForce(syntax);
                    break;

                //Add torque to object by ID (format: id x y z)
                case "AddTorque":
                    Command_AddTorque(syntax);
                    break;

                //Select camera rotation behaviour
                case "SelectCameraMode":
                    Command_SelectCameraMode(syntax);
                    break;

                //Set to camera target, which camera follow. If wroten "null" instead of ID - stop camera moving
                case "SetCameraTarget":
                    Command_SetCameraTarget(syntax);
                    break;

                //Select moving module for player
                case "SetPlayerMovingMode":
                    Command_SetPlayerMovingMode(syntax);
                    break;

                //Teleport object to position or object's position (format: id x y z)/(format: id targetId)
                case "TP":
                    Command_TP(syntax);
                    break;

                //Set objcet by id active/nonactive (format: id state)
                case "SetObjActive":
                    Command_SetObjActive(syntax);
                    break;

                //Set camera following offset (format: x y z)
                case "SetCameraOffset":
                    Command_SetCameraOffset(syntax);
                    break;

                //Set interaction module for player
                case "SetPlayerInteractionMode":
                    Command_SetPlayerInteractionMode(syntax);
                    break;

                //Set single value of view (format: valueType(x\y\z) newValue)
                case "SetViewSingle":
                    Command_SetViewSingle(syntax);
                    break;

                //Set new text to task window
                case "SetTask":
                    Command_SetTask(syntax);
                    break;

                //Hide or show cursor
                case "SetCursorVisible":
                    Command_SetCursorVisible(syntax);
                    break;

                //Play sound on position of object with id (format: id sound name)
                case "PlaySound":
                    Command_PlaySound(syntax);
                    break;

                //Stop or Start playing sound on audiosource, assigned to object by id (format: id On/Off)
                case "TurnAudioSource":
                    Command_TurnAudioSource(syntax);
                    break;

                default:
                    throw new AIADException($"Unknown command {syntax[0]}.", "SL_Executor.ExecuteCommandBySyntax");
            }
        }

        private static void Command_SetView(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_SetView()";

            if (Registry.MainCameraBehaviour == null)
                throw new AIADException("Missing MainCameraBehaviour.", ExcSrc);
            if (syntax.Length < 3)
                throw new AIADException("Command haven't any arguments", ExcSrc);

            AIADException CantParseValue()
            {
                return new AIADException("Cant parse argument in float value.", ExcSrc);
            }
            float x;
            float y;
            float z;
            if (!float.TryParse(syntax[1], out x) ||
                !float.TryParse(syntax[2], out y))
            {
                throw CantParseValue();
            }
            if (syntax.Length > 3)
            {
                if (!float.TryParse(syntax[3], out z))
                    throw CantParseValue();
            }
            else
            {
                z = 0;
            }
            Registry.MainCameraBehaviour.SetRotation(new Vector3(x, y, z));
        }
        private static void Command_TurnInteraction(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_TurnInteraction()";

            if (syntax.Length < 3)
                throw new AIADException("Haven't any arguments", ExcSrc);

            int id;

            if (!int.TryParse(syntax[1], out id))
                throw new AIADException("Cant parse id", ExcSrc);

            IInteractiveObject obj = (ObjectIDManager.GetObjectByID(id) as MonoBehaviour).GetComponentInParent<IInteractiveObject>();

            if (obj == null)
                throw new AIADException($"Haven't interactive object by ID= {id}.", ExcSrc);


            if (syntax[2] == "Off")
            {
                obj.TurnOffInteraction();
            }
            else if (syntax[2] == "On")
            {
                obj.TurnOnInteraction();
            }
            else
                throw new AIADException("Third argument must be Off/On.", ExcSrc);
        }
        private static void Command_Destroy(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_Destroy()";

            if (syntax.Length < 2)
                throw new AIADException("Haven't any argument.", ExcSrc);

            int id;

            if (!int.TryParse(syntax[1], out id))
                throw new AIADException("Can't parse id", ExcSrc);

            GameObject.Destroy((ObjectIDManager.GetObjectByID(id) as MonoBehaviour).gameObject);
        }
        private static void Command_Message(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_Message()";

            if (syntax.Length < 3)
                throw new AIADException("Haven't any argument.", ExcSrc);

            int id;

            if (!int.TryParse(syntax[1], out id))
                throw new AIADException("Can't parse id", ExcSrc);

            (ObjectIDManager.GetObjectByID(id) as MonoBehaviour).SendMessage(syntax[2]);
        }
        private static void Command_SetAnimTrigger(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_SetAnimTrigger()";

            if (syntax.Length < 3)
                throw new AIADException("Haven't any argument.", ExcSrc);

            int id;

            if (!int.TryParse(syntax[1], out id))
                throw new AIADException("Can't parse id", ExcSrc);

            Animator animator = (ObjectIDManager.GetObjectByID(id) as MonoBehaviour).GetComponentInParent<Animator>();

            if (animator == null)
                throw new AIADException($"Object by ID= {id} haven't Animator. ", ExcSrc);

            animator.SetTrigger(syntax[2]);
        }
        private static void Command_AddForce(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_AddForce()";


            if (syntax.Length < 5)
                throw new AIADException("Command haven't any arguments", ExcSrc);

            int id;
            if (!int.TryParse(syntax[1], out id))
                throw new AIADException("Cant parse ID. ", ExcSrc);

            AIADException CantParseValue()
            {
                return new AIADException("Cant parse argument in float value.", ExcSrc);
            }
            float x;
            float y;
            float z;
            if (!float.TryParse(syntax[2], out x) ||
                !float.TryParse(syntax[3], out y) ||
                !float.TryParse(syntax[4], out z))
            {
                throw CantParseValue();
            }
            Rigidbody rgbody = (ObjectIDManager.GetObjectByID(id) as MonoBehaviour).GetComponent<Rigidbody>();

            if (rgbody == null)
                throw new AIADException($"Object with ID= {id} haven't Rigidbody.", ExcSrc);

            rgbody.AddForce(new Vector3(x, y, z), ForceMode.Impulse);
        }
        private static void Command_AddTorque(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_AddTorque()";


            if (syntax.Length < 5)
                throw new AIADException("Command haven't any arguments", ExcSrc);

            int id;
            if (!int.TryParse(syntax[1], out id))
                throw new AIADException("Cant parse ID. ", ExcSrc);

            AIADException CantParseValue()
            {
                return new AIADException("Cant parse argument in float value.", ExcSrc);
            }
            float x;
            float y;
            float z;
            if (!float.TryParse(syntax[2], out x) ||
                !float.TryParse(syntax[3], out y) ||
                !float.TryParse(syntax[4], out z))
            {
                throw CantParseValue();
            }
            Rigidbody rgbody = (ObjectIDManager.GetObjectByID(id) as MonoBehaviour).GetComponent<Rigidbody>();

            if (rgbody == null)
                throw new AIADException($"Object with ID= {id} haven't Rigidbody.", ExcSrc);

            rgbody.AddTorque(new Vector3(x, y, z), ForceMode.Impulse);
        }
        private static void Command_SelectCameraMode(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_SelectCameraMode()";

            if (syntax.Length < 2)
                throw new AIADException("Haven't any argument.", ExcSrc);

            int modeIndex;

            if (!int.TryParse(syntax[1], out modeIndex))
                throw new AIADException("Can't parse mode index", ExcSrc);

            (Registry.MainCameraBehaviour as CameraBehaviourSelector).SelectBehaviour(modeIndex);
        }
        private static void Command_SetCameraTarget(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_SelectCameraMode()";

            if (syntax.Length < 2)
                throw new AIADException("Haven't any argument.", ExcSrc);

            Transform target;
            {
                int id;
                if (!int.TryParse(syntax[1], out id))
                {
                    if (syntax[1] != "null")
                        throw new AIADException("Can't parse id", ExcSrc);
                    else
                        target = null;
                }
                else
                    target = (ObjectIDManager.GetObjectByID(id) as MonoBehaviour).transform;
            }

            Registry.MainCameraMovingBehaviour.ChangeTarget(target);
        }
        private static void Command_SetPlayerMovingMode(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_SetPlayerMovingMode()";

            if (syntax.Length < 2)
                throw new AIADException("Haven't any argument.", ExcSrc);

            int modeIndex;

            if (!int.TryParse(syntax[1], out modeIndex))
                throw new AIADException("Can't parse mode index", ExcSrc);

            Registry.PlayerMovingModuleSelector.SelectModule(modeIndex);
        }
        private static void Command_TP(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_TP()";


            if (syntax.Length < 3)
                throw new AIADException("Command haven't any arguments", ExcSrc);

            int id;
            if (!int.TryParse(syntax[1], out id))
                throw new AIADException("Cant parse ID. ", ExcSrc);

            Vector3 tpPos;
            if (syntax.Length == 3)
            {
                int targetId;
                if (!int.TryParse(syntax[2], out targetId))
                    throw new AIADException("Cant parse ID of target.", ExcSrc);

                tpPos = (ObjectIDManager.GetObjectByID(targetId) as MonoBehaviour).transform.position;
            }
            else
            {
                AIADException CantParseValue()
                {
                    return new AIADException("Cant parse argument in float value.", ExcSrc);
                }

                float x;
                float y;
                float z;
                if (!float.TryParse(syntax[2], out x) ||
                    !float.TryParse(syntax[3], out y) ||
                    !float.TryParse(syntax[4], out z))
                {
                    throw CantParseValue();
                }
                tpPos = new Vector3(x, y, z);

            }
            (ObjectIDManager.GetObjectByID(id) as MonoBehaviour).transform.position = tpPos;
        }
        private static void Command_SetObjActive(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_SetObjActive()";

            if (syntax.Length < 3)
                throw new AIADException("Haven't any arguments", ExcSrc);

            int id;

            if (!int.TryParse(syntax[1], out id))
                throw new AIADException("Cant parse id", ExcSrc);

            GameObject obj = (ObjectIDManager.GetObjectByID(id) as MonoBehaviour).gameObject;
            if (obj == null)
                throw new AIADException($"Haven't object by ID= {id}.", ExcSrc);

            if (syntax[2] == "Off")
                obj.SetActive(false);
            else if (syntax[2] == "On")
                obj.SetActive(true);
            else
                throw new AIADException("Third argument must be Off/On.", ExcSrc);
        }
        private static void Command_SetCameraOffset(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_SetCameraOffset()";

            if (Registry.MainCameraBehaviour == null)
                throw new AIADException("Missing MainCameraBehaviour.", ExcSrc);
            if (syntax.Length < 4)
                throw new AIADException("Command haven't any arguments", ExcSrc);

            AIADException CantParseValue()
            {
                return new AIADException("Cant parse argument in float value.", ExcSrc);
            }
            float x;
            float y;
            float z;
            if (!float.TryParse(syntax[1], out x) ||
                !float.TryParse(syntax[2], out y) ||
                !float.TryParse(syntax[3], out z))
            {
                throw CantParseValue();
            }

            Registry.MainCameraMovingBehaviour.SetFollowingOffset(new Vector3(x, y, z));
        }
        private static void Command_SetPlayerInteractionMode(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_SetPlayerInteractionMode()";

            if (syntax.Length < 2)
                throw new AIADException("Haven't any argument.", ExcSrc);

            int modeIndex;

            if (!int.TryParse(syntax[1], out modeIndex))
                throw new AIADException("Can't parse mode index", ExcSrc);

            Registry.PlayerInteractionModuleSelector.SelectModule(modeIndex);
        }
        private static void Command_SetViewSingle(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_SetViewSingle()";

            if (Registry.MainCameraBehaviour == null)
                throw new AIADException("Missing MainCameraBehaviour.", ExcSrc);
            if (syntax.Length < 3)
                throw new AIADException("Command haven't any arguments", ExcSrc);

            char type;
            if (!char.TryParse(syntax[1], out type))
                throw new AIADException("Cant parse type of changed rotation axis.", ExcSrc);

            float value;
            if (!float.TryParse(syntax[2], out value))
                throw new AIADException("Cant parse rotation value argument.", ExcSrc);

            switch (type)
            {
                case 'x':
                    Registry.MainCameraBehaviour.SetXRotation(value);
                    break;
                case 'y':
                    Registry.MainCameraBehaviour.SetYRotation(value);
                    break;
                case 'z':
                    Registry.MainCameraBehaviour.SetZRotation(value);
                    break;

                default:
                    throw new AIADException($"Axis {type} doesn't exist.", ExcSrc);
            }
        }
        private static void Command_SetTask(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_SetTask()";

            if (syntax.Length < 2)
                throw new AIADException("Haven't any argument.", ExcSrc);

            StringBuilder str = new StringBuilder();
            for (int i = 1; i < syntax.Length; i++)
            {
                str.Append(syntax[i]);
                str.Append(' ');
            }
            Registry.TaskShower.SetNewTask(str.ToString());
        }
        private static void Command_SetCursorVisible(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_SetCursorVisible()";

            if (syntax.Length < 2)
                throw new AIADException("Haven't any arguments", ExcSrc);

            if (syntax[1] == "Off")
            {
                Cursor.visible = false;
            }
            else if (syntax[1] == "On")
            {
                Cursor.visible = true;
            }
            else
                throw new AIADException("Third argument must be Off/On.", ExcSrc);
        }
        private static void Command_PlaySound(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_PlaySound()";

            if (syntax.Length < 3)
                throw new AIADException("Haven't any argument.", ExcSrc);

            int id;

            if (!int.TryParse(syntax[1], out id))
                throw new AIADException("Can't parse id", ExcSrc);

            string clipPath =SoundsManager.SoundsPath + syntax[2];
            AudioClip clip = Resources.Load<AudioClip>(clipPath);
            MonoBehaviour ownObj = ObjectIDManager.GetObjectByID(id) as MonoBehaviour;

            if (clip == null)
                throw new AIADException($"Missing clip at path= {clipPath} .", ExcSrc);
            if (ownObj == null)
                throw new AIADException("Missing object by id= " + id, ExcSrc);

            OneShotSoundCreator.PlaySound(clip, ownObj.transform.position, ownObj.transform);
        }
        private static void Command_TurnAudioSource(string[] syntax)
        {
            string ExcSrc = "SL_Executor.Command_TurnAudioSource()";

            if (syntax.Length < 3)
                throw new AIADException("Haven't any arguments", ExcSrc);

            int id;

            if (!int.TryParse(syntax[1], out id))
                throw new AIADException("Cant parse id", ExcSrc);

            AudioSource src = (ObjectIDManager.GetObjectByID(id) as MonoBehaviour).GetComponent<AudioSource>();

            if (src == null)
                throw new AIADException($"Haven't audio source on object by ID= {id}.", ExcSrc);


            if (syntax[2] == "Off")
            {
                src.Stop();
            }
            else if (syntax[2] == "On")
            {
                src.Play();
            }
            else
                throw new AIADException("Third argument must be Off/On.", ExcSrc);
        }
    }
}
