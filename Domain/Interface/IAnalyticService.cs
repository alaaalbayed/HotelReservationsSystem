using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IAnalyticService
    {
        Task<int> TotalReservationsNumber();
        Task<double> TotalIncome();
        Task<int> TotalUsers();
        Task<int> TotalAdmins();
        Task<int> TotalEmployees();
        Task<double> GetMonthlySale(string year, int month);
        Task<double> GetMonthlyOrder(string year, int month);
    }
}
