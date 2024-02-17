using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.SL
{
    public sealed class ObjectUniqueID : MonoBehaviour, IObjectUniqueID
    {
        [SerializeField] private int ID;

        private void Awake()
        {
            ObjectIDManager.AddObject(this);
        }

        public int ID_ => ID;
    }
}
