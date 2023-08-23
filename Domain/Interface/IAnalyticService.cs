using Domain.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IAnalyticService
    {
        Task<int> TotalReservationsNumber();
        Task<double> TotalRevenue();
        Task<double> TotalIncome();
        Task<int> TotalUsers();
        Task<int> TotalAdmins();
        Task<int> TotalEmployees();
    }
}
