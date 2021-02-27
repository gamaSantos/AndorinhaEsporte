using System;

namespace AndorinhaEsporte.Domain
{
    public enum PlayerAction
    {
        Idle = 0,
        FollowingBall,
        Pass,
        Serve,
        Rotate,
        ChangeSides,
        ResetPosition,
        Defend,
        Spike
    }
}