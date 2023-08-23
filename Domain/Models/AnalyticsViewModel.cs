using Domain.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AnalyticsViewModel
    {
        public int TotalReservations { get; set; }
        public double TotalRevenue { get; set; }
        public double TotalIncome { get; set;}
        public int TotalUsers { get; set;}
        public int TotalAdmins { get; set;}
        public int TotalEmployees { get; set;}
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
