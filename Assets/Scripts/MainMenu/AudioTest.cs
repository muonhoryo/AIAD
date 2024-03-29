using AIAD.SL;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AIAD
{
    public sealed class AudioTest : MainMenuButton
    {
        private static AudioTest ActiveTest= null;

        [SerializeField] private AudioSource TestSource;

        protected override void OnPointerDownAction(PointerEventData eventData)
        {
            if (ActiveTest != this)
                StopActiveTest();
            TurnTest();
        }

        private bool IsPlayedTest() => TestSource.isPlaying;
        public void TurnTest()
        {
            if (IsPlayedTest())
                TestSource.Stop();
            else
            {
                TestSource.time = 0;
                TestSource.Play();
                ActiveTest = this;
            }
        }

        public static void StopActiveTest()
        {
            if (ActiveTest != null&& ActiveTest.IsPlayedTest())
            {
                ActiveTest.TurnTest();
                ActiveTest = null;
            }
        }
    }
}
