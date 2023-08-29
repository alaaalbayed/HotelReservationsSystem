using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Domain.Service
{
    public class AnalyticService : IAnalyticService
    {
        private readonly Ecommerce_AppContext _db;

        public AnalyticService(Ecommerce_AppContext db)
        {
            _db = db;
        }

        public async Task<int> TotalReservationsNumber()
        {
            var inactiveReservationsCount = await _db.Reservations
                   .Where(reservation => reservation.Status == false)
                   .CountAsync();

            return inactiveReservationsCount;
        }

        public async Task<double> TotalIncome()
        {
            var totalIncome = await _db.Reservations
                .Where(reservation => reservation.Status == false)
                .SumAsync(x => x.Price);
            return totalIncome;
        }

        public async Task<int> TotalUsers()
        {
            var totalUsers = await _db.AspNetUsers
                .Where(users => users.Status == false && users.Role.Any(x=>x.Name == "User"))
                .CountAsync();
            return totalUsers;
        }

        public async Task<int> TotalAdmins()
        {
            var totalAdmins = await _db.AspNetUsers
                .Where(user => user.Role.Any(role => role.Name == "Admin"))
                .CountAsync();

            return totalAdmins;
        }

        public async Task<int> TotalEmployees()
        {
            var totalEmployees = await _db.AspNetUsers
                .Where(user => user.Role.Any(role => role.Name == "Employee"))
                .CountAsync();

            return totalEmployees;
        }

        public async Task<double> GetMonthlySale(string year, int month)
        {
            try
            {
                int selectedYear = string.IsNullOrWhiteSpace(year) ? DateTime.Now.Year : int.Parse(year);

                var monthlySale = await _db.Reservations
                    .Where(x => x.OrderDate.Year == selectedYear && x.OrderDate.Month == month && x.Status == false)
                    .Select(x => x.Price)
                    .SumAsync();

                return monthlySale;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<double> GetMonthlyOrder(string year, int month)
        {
            try
            {
                int selectedYear = string.IsNullOrWhiteSpace(year) ? DateTime.Now.Year : int.Parse(year);

                var monthlySale = await _db.Reservations
                    .Where(x => x.OrderDate.Year == selectedYear && x.OrderDate.Month == month && x.Status == false)
                    .CountAsync();

                return monthlySale;
            }
            catch
            {
                return 0;
            }
        }
    }
}
