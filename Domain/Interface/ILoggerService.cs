namespace Domain.Interface
{
    public interface ILoggerService
    {
        void LogError(string message, Exception exception);
    }
}
