using AIAD.Player.COM;
using AIAD.Exceptions;
using UnityEngine;
using AIAD.SL;
using System.Collections;

namespace AIAD
{
    public sealed class Tutorial : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour ViewModuleComponent;
        [SerializeField] private MonoBehaviour MovingModuleComponent;
        [SerializeField] private MonoBehaviour InteractionModuleComponent;

        private IViewDirectionModule ViewModule;
        private IMovingModule MovingModule;
        private IInteractionModule InteractionModule;

        [SerializeField] private string SLScriptsIDS_View;
        [SerializeField] private string SLScriptsIDS_Moving;
        [SerializeField] private string SLScriptsIDS_Interaction;

        [SerializeField] private float MaxViewDotProduct;
        [SerializeField] private float MinMovingDistance;

        private Vector3 PrevDirection;
        private Vector3 PrevPosition;

        private void InteractionAction(IInteractiveObject i)
        {
            if (!string.IsNullOrEmpty(SLScriptsIDS_Interaction))
            {
                SLScriptsIDS_Interaction.RunSLScripts();
                InteractionModule.InteractionEvent -= InteractionAction;
            }
        }

        private void StopMovingAction()
        {
            enabled = false;
        }
        private void OnStartMovingAction(Vector3 i)
        {
            enabled = true;
        }

        private void ChangeViewAction(Vector3 viewDir,Vector2 i)
        {
            if (Vector3.Dot(viewDir, PrevDirection) <= MaxViewDotProduct)
            {
                if (!string.IsNullOrEmpty(SLScriptsIDS_View))
                    SLScriptsIDS_View.RunSLScripts();
                ViewModule.ChangeViewDirectionEvent -= ChangeViewAction;
            }
        }
        private void Update()
        {
            if (Vector3.Distance(MovingModule.CurrentMovableObjectPosition_, PrevPosition) >= MinMovingDistance)
            {
                if (!string.IsNullOrEmpty(SLScriptsIDS_Moving))
                {
                    SLScriptsIDS_Moving.RunSLScripts();
                    MovingModule.StartMovingEvent -= OnStartMovingAction;
                    MovingModule.StopMovingEvent -= StopMovingAction;
                    enabled = false;
                }
            }
        }

        private void Awake()
        {
            enabled = false;
        }

        public void StartTutorial()
        {
            string ExcSrc = "Tutorial.Awake()";

            ViewModule = ViewModuleComponent as IViewDirectionModule;
            MovingModule = MovingModuleComponent as IMovingModule;
            InteractionModule = InteractionModuleComponent as IInteractionModule;

            if (ViewModule == null)
                throw new AIADException("Missing viewModule.", ExcSrc);
            if (MovingModule == null)
                throw new AIADException("Missing movingModule.", ExcSrc);
            if (InteractionModule == null)
                throw new AIADException("Missing interactionModule.", ExcSrc);

            PrevDirection = ViewModule.CurrentViewDirection_;
            PrevPosition = MovingModule.CurrentMovableObjectPosition_;

            ViewModule.ChangeViewDirectionEvent += ChangeViewAction;
            MovingModule.StartMovingEvent += OnStartMovingAction;
            MovingModule.StopMovingEvent += StopMovingAction;
            InteractionModule.InteractionEvent += InteractionAction;
        }
    }
}
