using Domain.DTO_s;

namespace Domain.Interface
{
    public interface IReservationService
    {
        Task Add(Reservation reservation, string userId);
        Task Update(int reservationId, Reservation updatedReservation, List<Escort> escorts);
        Task Delete(int reservationId);
        Task<Reservation> GetReservationById(int reservationId);
        Task<IEnumerable<Reservation>> GetAllReservations();
        Task<bool> AreDatesAcceptable(int roomId, DateTime CheckIn, DateTime CheckOut, int? reservationId);
        int GetDaysBetweenDates(DateTime chickIn, DateTime chickOut);
        Task<double> GetFinalPrice(Reservation reservation, List<Escort> escorts);

    }
}
