using AIAD.Exceptions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.SL
{
    public static class ObjectIDManager 
    {
        private static Dictionary<int,IObjectUniqueID> IdentifiedObjectsList= new Dictionary<int,IObjectUniqueID>();
        public static IObjectUniqueID GetObjectByID(int id)
        {
            return IdentifiedObjectsList[id];
        }
        public static void AddObject(IObjectUniqueID obj)
        {
            if (IdentifiedObjectsList.ContainsKey(obj.ID_))
                IdentifiedObjectsList.Remove(obj.ID_);

            IdentifiedObjectsList.Add(obj.ID_, obj);
        }
    }
}
