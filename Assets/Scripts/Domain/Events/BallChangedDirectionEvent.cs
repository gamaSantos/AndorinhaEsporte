using System;
using UnityEngine;

namespace AndorinhaEsporte.Domain.Events
{
    public class BallChangedDirectionEventArgs : EventArgs
    {
        public Vector3? LandingSpot;
        public Guid TeamId;
        public float? FowardDirection => LandingSpot?.z;
    }
}