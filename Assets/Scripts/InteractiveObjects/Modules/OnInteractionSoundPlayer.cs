
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD
{
    public sealed class OnInteractionSoundPlayer : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour InteractiveObjComponent;
        [SerializeField] private GameObject SoundPrefab;
        [SerializeField] private AudioClip PlayedSound;

        private void InteractionAction()
        {
            OneShotSoundCreator.PlaySound(SoundPrefab, PlayedSound, InteractiveObjComponent.transform.position, InteractiveObjComponent.transform);
        }
        private void Awake()
        {
            string ExcSrc = "OnInteractionSoundPlayer.Awake()";

            IInteractiveObject obj = InteractiveObjComponent as IInteractiveObject;

            if (obj == null)
                throw new AIADException("Missing Interactive object.", ExcSrc);
            if (SoundPrefab == null)
                throw new AIADException("Missing SoundPrefab.", ExcSrc);
            if (PlayedSound == null)
                throw new AIADException("Missing PlayedSound.", ExcSrc);

            obj.InteractionEvent += InteractionAction;
        }
    }
}
