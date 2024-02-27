using AIAD.Exceptions;
using System.Collections;
using UnityEngine;

namespace AIAD
{
    public sealed class LampFlashing_Fading : MonoBehaviour
    {
        [SerializeField] private Light PointLight;
        [SerializeField] private MeshRenderer LightingMesh;
        [SerializeField] private AudioSource SoundSource;

        [SerializeField] private float FadingSpeed;
        [SerializeField] private float IntensityMin;
        [SerializeField] private float FadingInterval_Min;
        [SerializeField] private float FadingInterval_Max;

        private float IntensityMax;

        private void Awake()
        {
            string ExcSrc = "LampFlashin_Fading.Awake()";

            if (PointLight == null)
                throw new AIADException("Haven't Light component.", ExcSrc);
            if (LightingMesh == null)
                throw new AIADException("Haven't Mesh component.", ExcSrc);
            if (FadingSpeed <= 0)
                throw new AIADException("FadingSpeed must be greater than zero.", ExcSrc);
            if (IntensityMin < 0)
                throw new AIADException("IntensityMin must be equal or greater than zero.", ExcSrc);
            if(FadingInterval_Min<=0)
                throw new AIADException("FadingInterval_Min must be greater than zero.", ExcSrc);
            if (FadingInterval_Max <= 0)
                throw new AIADException("FadingInterval_Max must be greater than zero.", ExcSrc);
            if (FadingInterval_Max < FadingInterval_Min)
                throw new AIADException("FadingInterval_Max cannot be less than FadingInterval_Min.", ExcSrc);

            IntensityMax = PointLight.intensity;

            if(IntensityMin>IntensityMax)
                throw new AIADException("IntensityMin cannot be greater than IntensityMax.", ExcSrc);

            StartCoroutine(Fading());
        }

        private IEnumerator Fading()
        {
            void SetIntensity(float intensity)
            {
                PointLight.intensity=intensity;
                float strentgh = intensity / IntensityMax;
                LightingMesh.material.color = new Color(strentgh, strentgh, strentgh);
            }
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(FadingInterval_Min, FadingInterval_Max));
                SoundSource.Play();
                while (PointLight.intensity>IntensityMin)
                {
                    float newIntensity = PointLight.intensity - FadingSpeed;
                    if (newIntensity > IntensityMin)
                    {
                        SetIntensity(newIntensity);
                        yield return new WaitForEndOfFrame();
                    }
                    else
                    {
                        SetIntensity(IntensityMin);
                        break;
                    }
                }
                yield return new WaitForEndOfFrame();
                while (PointLight.intensity < IntensityMax)
                {
                    float newIntensity = PointLight.intensity + FadingSpeed;
                    if (newIntensity < IntensityMax)
                    {
                        SetIntensity(newIntensity);
                        yield return new WaitForEndOfFrame();
                    }
                    else
                    {
                        SetIntensity(IntensityMax);
                        break;
                    }
                }
            }
        }
        
    }

}
