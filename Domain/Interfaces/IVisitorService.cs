using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IVisitorService
    {
        Task<bool> HasVisitedToday(string ipAddress, DateTime visitDate);
        Task AddOrUpdateVisitor(string ipAddress, DateTime visitDate);
        Task<int> GetVisitorsCount();
    }
}
