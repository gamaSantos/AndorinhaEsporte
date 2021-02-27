namespace AndorinhaEsporte.Domain.State
{
    public enum ServeStateEnum
    {
        Initial = 0,
        Moving,
        AwaitingApproval,
        Approved,
        Serving,
        Finished
    }
}
