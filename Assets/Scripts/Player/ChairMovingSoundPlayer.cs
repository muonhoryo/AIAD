
using UnityEngine;
using AIAD.Exceptions;
using AIAD.Player.COM;

namespace AIAD.Player
{
    public sealed class ChairMovingSoundPlayer : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour MovingModuleComponent;
        [SerializeField] private AudioSource SoundSource;

        private void StopMovingAction()
        {
            SoundSource.Stop();
        }
        private void StartMovingAction(Vector3 i)
        {
            SoundSource.Play();
        }
        private void Awake()
        {
            string ExcSrc = "ChairMovingSoundPlayer.Awake()";

            IMovingModule movingModule = MovingModuleComponent as IMovingModule;

            if (movingModule == null)
                throw new AIADException("Missing MovingModule.", ExcSrc);
            if (SoundSource == null)
                throw new AIADException("Missing PlayedSound.", ExcSrc);

            movingModule.StartMovingEvent += StartMovingAction;
            movingModule.StopMovingEvent += StopMovingAction;
        }
    }
}
