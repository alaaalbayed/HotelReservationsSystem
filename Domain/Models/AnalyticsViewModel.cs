using Domain.DTO_s;

namespace Domain.Models
{
    public class AnalyticsViewModel
    {
        public int TotalReservations { get; set; }
        public double TotalIncome { get; set;}
        public int TotalUsers { get; set;}
        public int TotalAdmins { get; set;}
        public int TotalEmployees { get; set;}
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
