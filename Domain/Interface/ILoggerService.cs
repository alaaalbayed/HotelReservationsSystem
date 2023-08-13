using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ILoggerService
    {
        void LogError(string message, Exception exception);
    }
}
