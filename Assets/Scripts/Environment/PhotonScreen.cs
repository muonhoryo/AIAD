
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD
{
    public sealed class PhotonScreen : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour SubscribeTargetComponent;
        [SerializeField] private BinaryActivityStateObject Owner;

        private void Awake()
        {
            string ExcSrc = "PhotonScreen.Awake()";

            IBinaryActivStateObj subscribeTarget = SubscribeTargetComponent as IBinaryActivStateObj;

            if (subscribeTarget == null)
                throw new AIADException("Cant parse SubscribeTargetComponent.", ExcSrc);
            if (Owner == null)
                throw new AIADException("Missing OwnerComponent.", ExcSrc);

            subscribeTarget.ActivationEvent += Owner.TurnOn;
            subscribeTarget.DeactivationEvent += Owner.TurnOff;
        }
    }
}
