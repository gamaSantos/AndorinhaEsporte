namespace AndorinhaEsporte.CommandHandlers
{
    public class CommandResult<T> where T : class
    {
        public CommandResult(T data)
        {
            Success = true;
            Data = data;
            ErrorMessage = string.Empty;
        }

        public CommandResult(string message, T data = null)
        {
            Success = false;
            Data = data;
            ErrorMessage = message;
        }

        public bool Success { get; }
        public T Data { get; }
        public string ErrorMessage { get; }
    }
}