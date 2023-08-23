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
