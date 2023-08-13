using Domain.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IEscortService
    {
        Task Add(int reservationId, List<Escort> escorts);

        Task Remove(int Id);

        Task Update(int Id, Escort escort);

        Task<List<Escort>> GetAllEscorts(Escort escort);

        List<Escort> GetEscorts(int reservationId);
        Task UpdateEscorts(int reservationId, List<Escort> newEscorts);

    }
}
