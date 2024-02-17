
using AIAD.Exceptions;
using UnityEngine;
using MuonhoryoLibrary.Unity;

namespace AIAD.Player.COM
{
    public sealed class MovDirFromViewDirModule : MonoBehaviour,IMovDirCalcModule
    {
        [SerializeField] private MonoBehaviour ViewModuleComponent;

        private IViewDirectionModule ViewModule;

        private AIADException MissingViewModuleException(string calledFuncName) =>
            new AIADMissingModuleException("ViewModule", $"MovDirFromViewDirModule.{calledFuncName}()");
        private void Awake()
        {
            ViewModule = ViewModuleComponent as IViewDirectionModule;
            if (ViewModule == null)
                throw MissingViewModuleException("Awake");
        }
        Vector3 IMovDirCalcModule.GetDirection(float HorizontalAxisValue, float VerticalAxisValue)
        {
            if (ViewModule == null)
                throw MissingViewModuleException("IMovDirCalcModule.GetDirection");
            Vector2 viewDir = ViewModule.CurrentHorizontalViewDirection_;
            HorizontalAxisValue = -Mathf.Floor(HorizontalAxisValue);
            VerticalAxisValue=Mathf.Floor(VerticalAxisValue);
            Vector2 movDir= (viewDir*VerticalAxisValue+Vector2.Perpendicular(viewDir)* HorizontalAxisValue).normalized;
            return new Vector3(movDir.x, 0, movDir.y);
        }
    }
}
