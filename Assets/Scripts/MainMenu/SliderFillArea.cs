using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AIAD
{
    public sealed class SliderFillArea : MonoBehaviour
    {
        [SerializeField] private Image BarImage;
        [SerializeField] private AnimationCurve SliderValueToThresholdFunc;
        [SerializeField] private Slider OwnedSlider;
        [SerializeField] private string ThresholdName;

        private void Awake()
        {
            BarImage.material = Instantiate(BarImage.material);
            ValueChangedAction();
        }

        public void ValueChangedAction()
        {
            BarImage.material.SetFloat(ThresholdName, SliderValueToThresholdFunc.Evaluate(OwnedSlider.value));
        }
    }
}
