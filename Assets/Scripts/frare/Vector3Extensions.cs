using NUnit.Framework.Constraints;
using UnityEngine;

namespace frare.Vector3Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 XY(this Vector3 v)
        {
            return new Vector3(v.x, v.y, 0f);
        }

        public static Vector3 XZ(this Vector3 v)
        {
            return new Vector3(v.x, 0f, v.z);
        }

        public static Vector3 YZ(this Vector3 v)
        {
            return new Vector3(0f, v.y, v.z);
        }

        public static bool Approximately(Vector3 a, Vector3 b, float tolerance = 0.0001f)
        {
            return (a - b).sqrMagnitude <= tolerance * tolerance;
        }
    }
}