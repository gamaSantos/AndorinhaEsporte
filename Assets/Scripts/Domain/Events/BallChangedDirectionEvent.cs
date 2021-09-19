using System;
using UnityEngine;

namespace AndorinhaEsporte.Domain.Events
{
    public class BallChangedDirectionEvent
    {
        public Vector3? LandingSpot;
        public Guid LastTouchTeamId;
        public float? FowardDirection => LandingSpot?.z;
    }
}