using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AIAD
{
    public static class Extensions
    {
        /// <summary>
        /// x;y -> x=x;0;z=y
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 Convert2DXYto3DXZ(this Vector2 v) =>
            new Vector3(v.x, 0, v.y);
        public static Vector3 Convert2DXYto3DXZ(this Vector3 v) =>
            ((Vector2)v).Convert2DXYto3DXZ();
        public static Vector2 Convert3DXZto2DXY(this Vector3 v) =>
            new Vector2(v.x, v.z);

        public static Vector3 DirectionFromAngle3D(this Vector2 eulerAngles)
        {
            float xRadAngle = eulerAngles.x * Mathf.Deg2Rad;
            float yRadAngle=eulerAngles.y * Mathf.Deg2Rad;
            return new Vector3
                (Mathf.Sin(yRadAngle) * Mathf.Cos(xRadAngle),
                 -Mathf.Sin(xRadAngle),
                Mathf.Cos(yRadAngle) * Mathf.Cos(xRadAngle));
        }
        public static Vector3 DirectionFromAngle3D(this Vector3 eulerAngles) =>
            ((Vector2)eulerAngles).DirectionFromAngle3D();
        public static string FilterBy(this string input, params char[] removedSymbols)
        {
            bool ComparionFunc(char sym)
            {
                foreach (char c in removedSymbols)
                {
                    if (sym == c) return false;
                }
                return true;
            }
            StringBuilder str = new StringBuilder(input.Length);
            foreach (var c in input)
            {
                if (ComparionFunc(c))
                {
                    str.Append(c);
                }
            }
            return str.ToString();
        }
    }
}
namespace AIAD.SL
{
    public static class SLExtensions 
    {
        public static void RunSLScripts(this string Ids)
        {
            string[] idList = Ids.Split(',');
            foreach (string id in idList)
            {
                SL_ScriptManager.ExecuteScriptById(id);
            }
        }
    }
}
