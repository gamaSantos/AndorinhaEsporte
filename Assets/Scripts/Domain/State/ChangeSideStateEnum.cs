namespace AndorinhaEsporte.Domain.State
{
    public enum ChangeSideStateEnum
    {
        Initial = 0,
        Queueing,
        ChangingSides,
        MovingToPosition,
        Finished
    }
}
