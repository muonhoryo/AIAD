using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD
{
    public interface ICameraStaggerValueSource 
    {
        public Vector3 StaggerRotationOffset_ { get; }
    }
}
