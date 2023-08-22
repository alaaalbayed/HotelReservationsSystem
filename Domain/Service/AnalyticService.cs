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
            var reservationsNumber = await _db.Reservations.CountAsync();
            return reservationsNumber;
        }

        public async Task<double> TotalRevenue()
        {
            var totalRevenue = await _db.Reservations.SumAsync(x => x.Price);
            return totalRevenue;
        }
        public async Task<double> TotalIncome()
        {
            var totalIncome = await _db.Reservations.SumAsync(x => x.Price);
            var tax = totalIncome * 0.16;
            return totalIncome - tax;
        }
    }
}
