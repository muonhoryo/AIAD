
using static AIAD.ExternalConsts;
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD.Player.COM
{
    public sealed class HPDecreaserSpeedUpBoost : MonoBehaviour
    {
        [SerializeField] private HitPointDecreaser OwnedScript;

        private void Awake()
        {
            string ExcSrc = "HPDecreaserSpeedUpBoost.Awake()";

            if (OwnedScript == null)
                throw new AIADException("Missing OwnedScript.", ExcSrc);
        }

        public void BoostDecreasing()
        {
            float remainedTime = OwnedScript.HPModule_.CurrentHP_ / (OwnedScript.DecreasingPointCount_ / OwnedScript.DecreasingTimeInterval_);
            if (remainedTime > Consts_.DecBooster_MaxRemTime)
            {
                float newDecreasingSpeed = OwnedScript.HPModule_.CurrentHP_ / Consts_.DecBooster_MaxRemTime;
                OwnedScript.SetDecreasingSpeed(newDecreasingSpeed);
            }
        }
    }
}
