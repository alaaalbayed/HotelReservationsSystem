using Domain.DTO_s;

namespace Domain.Models
{
    public class AnalyticsViewModel
    {
        public int TotalReservations { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public double TotalIncome { get; set;}
        public int TotalUsers { get; set;}
        public int TotalAdmins { get; set;}
        public int TotalEmployees { get; set;}
        public int TotalVisitors { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
