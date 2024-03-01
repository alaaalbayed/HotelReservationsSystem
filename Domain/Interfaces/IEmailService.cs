using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IEmailService
    {
        void SendEmail(string from, string to, string subject, string body);
    }
}
