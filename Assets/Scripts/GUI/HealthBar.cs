
using AIAD.Player.COM;
using UnityEngine;
using AIAD.Exceptions;
using UnityEngine.UI;

namespace AIAD
{
    public sealed class HealthBar : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour HPModuleComponent;
        [SerializeField] private Image MaterialOwner;
        [SerializeField] private Color LowHPColor;
        [SerializeField] private string ThresholdName;
        [SerializeField] private string ColorName;

        private void Awake()
        {
            string ExcSrc = "HealthBar.Awake()";

            IHitPointModule HPModule = HPModuleComponent as IHitPointModule;
            void HPChangedAction(int hp)
            {
                MaterialOwner.material.SetFloat(ThresholdName,(float)hp/HPModule.MaxHP_ );
            }
            void HPAchiviedZeroAction(int hp)
            {
                MaterialOwner.material.SetColor(ColorName, LowHPColor);
            }

            if (HPModule == null)
                throw new AIADException("Missing HPModule.", ExcSrc);
            if (MaterialOwner == null)
                throw new AIADException("Missing MaterialOwner.", ExcSrc);

            MaterialOwner.material = Instantiate(MaterialOwner.material);

            HPModule.HPCountHasChangedEvent += HPChangedAction;
            HPModule.LowHPCountHasAchievedEvent += HPAchiviedZeroAction;
        }
    }
}
