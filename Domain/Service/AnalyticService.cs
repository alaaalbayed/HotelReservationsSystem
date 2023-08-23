using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<double> TotalRevenue()
        {
            var totalRevenue = await _db.Reservations
                .Where(reservation => reservation.Status == false)
                .SumAsync(x => x.Price);
            return totalRevenue;
        }
        public async Task<double> TotalIncome()
        {
            var totalIncome = await _db.Reservations
                .Where(reservation => reservation.Status == false)
                .SumAsync(x => x.Price);
            var tax = totalIncome * 0.16;
            return totalIncome - tax;
        }

        public async Task<int> TotalUsers()
        {
            var totalUsers = await _db.AspNetUsers
                .Where(users => users.Status == false)
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
    }
}
