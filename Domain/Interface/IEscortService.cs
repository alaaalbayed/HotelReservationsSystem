using Domain.DTO_s;

namespace Domain.Interface
{
    public interface IEscortService
    {
        Task Add(int reservationId, List<Escort> escorts);
        Task<List<Escort>> GetEscorts(int reservationId);
        Task UpdateEscorts(int reservationId, List<Escort> newEscorts);

    }
}
