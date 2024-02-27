using System.Collections;
using AIAD.Exceptions;
using UnityEngine;

namespace AIAD
{
    public sealed class LampFlashing_TurningOff : MonoBehaviour
    {
        [SerializeField] private Light PointLight;
        [SerializeField] private MeshRenderer LightingMesh;
        [SerializeField] private AudioSource SoundSource;

        [SerializeField] private float TurningInterval_Min;
        [SerializeField] private float TurningInterval_Max;
        [SerializeField] private float TurningOffDuration_Min;
        [SerializeField] private float TurningOffDuration_Max;

        private void Awake()
        {
            string ExcSrc = "LampFlasginTurningOff";

            if (PointLight == null)
                throw new AIADException("Haven't PointLight.", ExcSrc);
            if (LightingMesh == null)
                throw new AIADException("Haven't LigthingMesh.", ExcSrc);
            if (TurningInterval_Min <= 0)
                throw new AIADException("TurningInterval_Min must be greater than zero.", ExcSrc);
            if (TurningInterval_Max <= 0)
                throw new AIADException("TurningInterval_Max must be greater than zero.", ExcSrc);
            if (TurningInterval_Min > TurningInterval_Max)
                throw new AIADException("TurningInterval_Max must be greater than TurningInterval_Min", ExcSrc);
            if (TurningOffDuration_Min <= 0)
                throw new AIADException("TurningOffDuration_Min must be greater than zero.", ExcSrc);
            if (TurningOffDuration_Max <= 0)
                throw new AIADException("TurningOffDuration_Max must be greater than zero.", ExcSrc);
            if (TurningOffDuration_Min > TurningOffDuration_Max)
                throw new AIADException("TurningOffDuration_Max must be greater than TurningOffDuration_Min", ExcSrc);

            StartCoroutine(TurningOff());
        }

        private IEnumerator TurningOff() 
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(TurningInterval_Min, TurningInterval_Max));
                SoundSource.Play();
                LightingMesh.material.color = new Color(0, 0, 0);
                PointLight.enabled = false;
                yield return new WaitForSeconds(Random.Range(TurningOffDuration_Min, TurningOffDuration_Max));
                LightingMesh.material.color = new Color(1,1, 1);
                PointLight.enabled = true;
            }
        }
    }

}
