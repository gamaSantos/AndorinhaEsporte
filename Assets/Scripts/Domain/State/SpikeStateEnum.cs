namespace AndorinhaEsporte.Domain.State
{
    public enum SpikeStateEnum
    {
        Initial = 0,
        Moving,
        AwaitingOrMovingToPassTarget,
        PreJump,
        Jumping,
        Finished
    }
}
