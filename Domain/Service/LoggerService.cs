using System.Diagnostics;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Domain.DTO_s;
using Domain.MAPPER;

namespace YourApplication.Infrastructure.Logging
{
    public class LoggerService : ILoggerService
    {
        private readonly string _logFilePath;
        private readonly Ecommerce_AppContext _dbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(string logFilePath, ILogger<LoggerService> logger, Ecommerce_AppContext dbContext, IServiceScopeFactory serviceScopeFactory)
        {
            _logFilePath = logFilePath;
            _dbContext = dbContext;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public void LogError(string message, Exception exception)
        {
            try
            {
                Log("Error", message, exception);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while logging an error message.", ex);
            }
        }

        public void Log(string level, string message, Exception exception = null)
        {
            try
            {
                var timestamp = DateTime.Now;
                var methodName = GetMethodName(exception);
                var className = GetClassName(exception);
                LogToDatabase(level, timestamp, exception, className, methodName, message);
                LogToFile(level, timestamp, exception, className, methodName, message);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while logging a message.", ex);
            }
        }

        private void LogToDatabase(string level, DateTime timestamp, Exception exception, string className, string methodName, string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var log = new Log
                {
                    Level = level,
                    Class = className,
                    Method = methodName,
                    Exception = exception?.Message ?? "None",
                    Message = message,
                    Timestamp = timestamp,
                };

                _dbContext.Logs.Add(MapLog.MAP(log));
                _dbContext.SaveChanges();
            }
        }

        private void LogToFile(string level, DateTime timestamp, Exception exception, string className, string methodName, string message)
        {
            using (var writer = new StreamWriter(_logFilePath, true))
            {
                writer.WriteLine("Level: " + level);
                writer.WriteLine("Class: " + (className ?? "None"));
                writer.WriteLine("Method: " + (methodName ?? "None"));
                writer.WriteLine("Exceptions: " + (exception?.Message ?? "None"));
                writer.WriteLine("Message: " + message);
                writer.WriteLine("Timestamp: " + timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                writer.WriteLine("-------------------------------------");
                writer.WriteLine();
            }
        }

        private string GetMethodName(Exception exception)
        {
            if (exception == null)
            {
                return null;
            }

            var stackTrace = new StackTrace(exception, true);
            var frames = stackTrace.GetFrames();

            if (frames != null && frames.Length > 0)
            {
                var method = frames[0].GetMethod();
                if (method != null)
                {
                    return method.Name;
                }
            }

            return null;
        }

        private string GetClassName(Exception exception)
        {
            if (exception == null)
            {
                return null;
            }

            var stackTrace = new StackTrace(exception, true);
            var frames = stackTrace.GetFrames();

            if (frames != null && frames.Length > 0)
            {
                var method = frames[0].GetMethod();
                if (method != null)
                {
                    return method.ReflectedType?.FullName;
                }
            }

            return null;
        }
    }
}
