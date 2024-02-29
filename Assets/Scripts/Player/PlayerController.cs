
using UnityEngine;
using AIAD.Player.COM;
using AIAD.Exceptions;

namespace AIAD.Player
{
    public sealed class PlayerController : MonoBehaviour,ILockableModule
    {

        [SerializeField] private string InputName_HorizontalMovingAxis;
        [SerializeField] private string InputName_VerticalMovingAxis;
        [SerializeField] private string InputName_MouseXAxis;
        [SerializeField] private string InputName_MouseYAxis;
        [SerializeField] private string InputName_Interaction;

        [SerializeField] private MonoBehaviour MovingModuleComponent;
        [SerializeField] private MonoBehaviour ViewDirectionModuleComponent;
        [SerializeField] private MonoBehaviour InteractionModuleComponent;

        private IMovingModule MovingModule;
        private IViewDirectionModule ViewDirectionModule;
        private IInteractionModule InteractionModule;

        private void Awake()
        {
            MovingModule = MovingModuleComponent as IMovingModule;
            ViewDirectionModule = ViewDirectionModuleComponent as IViewDirectionModule;
            InteractionModule=InteractionModuleComponent as IInteractionModule;

            Registry.PLayerController = this;
        }


        private float GetHorMovingAxis()
        {
            return Input.GetAxisRaw(InputName_HorizontalMovingAxis);
        }
        private float GetVertMovingAxis()
        {
            return Input.GetAxisRaw(InputName_VerticalMovingAxis);
        }
        private void MovingAction()
        {
            string GetExceptionSource() => "PlayerController.MovingAction()";

            void ThrowMisssing(string moduleName)
            {
#if UNITY_EDITOR
                Debug.LogWarning(AIADMissingModuleException.MissingModuleMessage_Full(moduleName, GetExceptionSource()));
            }
            if (MovingModule == null)
            {
                ThrowMisssing("MovingModule");
            }
#endif
            if (MovingModule == null )
            {
                return;
            }


            float hor = GetHorMovingAxis();
            float vert = GetVertMovingAxis();
            if (hor != 0 || vert != 0)
            {
                MovingModule.SetMovingDirection(new Vector2(hor, vert));
            }
            else
            {
                MovingModule.StopMoving();
            }
        }

        private float GetHorViewAxis()
        {
            return Input.GetAxisRaw(InputName_MouseXAxis);
        }
        private float GetVertViewAxis()
        {
            return Input.GetAxisRaw(InputName_MouseYAxis);
        }
        private void ViewChangingAction()
        {
            if (ViewDirectionModule == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning(AIADMissingModuleException.MissingModuleMessage_Full("ViewDirectionModule", "PlayerController.ViewChangingAction()"));
#endif
                return;
            }
            ViewDirectionModule.ChangeViewDirection(GetHorViewAxis(), GetVertViewAxis());
        }

        private void Interaction()
        {
            if (InteractionModule==null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("InteractionModule is not assigned. Source: PlayerController.Interaction().");
#endif
                return;
            }
            if (Input.GetButtonDown(InputName_Interaction))
            {
                InteractionModule.Interact();
            }
        }

        private void Update()
        {
            MovingAction();
            ViewChangingAction();
            Interaction();
        }

        void ILockableModule.Lock() => enabled = false;
        void ILockableModule.Unlock() => enabled = true;
        bool ILockableModule.IsLocked_ => !enabled;
    }
}

