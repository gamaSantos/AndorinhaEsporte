using System;
using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public static class Vector3Extensions
    {
        public static bool IsInCourt(this Vector3 position)
        {
            var isInLateralRange = position.x > -4.5f && position.x < 4.5f;
            var isInVerticalRange = position.z > -9 && position.z < 9f;
            return isInLateralRange && isInVerticalRange;
        }

        public static bool IsInPlayField(this Vector3 position)
        {
            var isInLateralRange = position.x > -9.2f && position.x < 9.1f;
            var isInVerticalRange = position.z > -12.6 && position.z < 12.6f;
            return isInLateralRange && isInVerticalRange;
        }

        public static float Distance(this Vector3 position, Vector3 target)
        {
            return Vector3.Distance(target, position);
        }

        public static Vector3 DirectionTo(this Vector3 position, Vector3 target)
        {
            var heading = target - position;
            return heading / heading.magnitude;
        }
    }
}