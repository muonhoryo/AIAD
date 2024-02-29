using AIAD.Player;
using AIAD.Player.COM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public static class Registry 
    {
        public static IControlLockModule ControlLocker;
        public static ICameraViewBehaviour MainCameraBehaviour;
        public static CameraTargetMoving MainCameraMovingBehaviour;
        public static MovingModuleSelector PlayerMovingModuleSelector;
        public static InteractionModuleSelector PlayerInteractionModuleSelector;
        public static PlayerController PLayerController;
        public static DebugConsoleController DebugConsoleController;
        public static TaskShower TaskShower;
        public static MusicChanger MusicChanger;
    }
}
