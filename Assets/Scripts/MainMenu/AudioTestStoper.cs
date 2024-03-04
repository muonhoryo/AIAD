using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public sealed class AudioTestStoper : MonoBehaviour
    {
        public void StopActiveTest()
        {
            AudioTest.StopActiveTest();
        }
    }
}
