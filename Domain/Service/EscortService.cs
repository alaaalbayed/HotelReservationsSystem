using Domain.DTO_s;
using Domain.Interface;
using Domain.MAPPER;
using Infrastructure.Data;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Domain.Service
{
    public class EscortService : IEscortService
    {
        private readonly Ecommerce_AppContext _db;
        public EscortService(Ecommerce_AppContext db)
        {
            _db = db;
        }

        public async Task Add(int reservationId, List<Escort> escorts)
        {
            if (escorts != null && escorts.Any())
            {
                foreach (var escort in escorts)
                {
                    var newEscort = new Escort
                    {
                        FullName = escort.FullName,
                        IsAdult = escort.IsAdult,
                        ReservationId = reservationId
                    };

                    var newEscortEntity = MapEscort.MAP(newEscort);
                    newEscortEntity.ReservationId = reservationId;
                    _db.Escorts.Add(newEscortEntity);
                }

                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Escort>> GetEscorts(int reservationId)
        {
            var list = new List<Escort>();

            var reservation = await _db.Escorts
                .Where(r => r.ReservationId == reservationId)
                .Select(r => new Escort
                {
                    FullName = r.FullName,
                    IsAdult = r.IsAdult,
                    ReservationId = r.ReservationId
                })
                .ToListAsync();

            list.AddRange(reservation);

            return list;
        }

        public async Task UpdateEscorts(int reservationId, List<Escort> newEscorts)
        {
            var existingEscorts = await _db.Escorts
                .Where(e => e.ReservationId == reservationId)
                .ToListAsync();

            var escortsToDelete = existingEscorts
                .Where(existingEscort => !newEscorts.Any(newEscort => newEscort.EscortId == existingEscort.EscortId))
                .ToList();

            _db.Escorts.RemoveRange(escortsToDelete);

            foreach (var newEscort in newEscorts)
            {
                var existingEscort = existingEscorts.FirstOrDefault(e => e.EscortId == newEscort.EscortId);
                if (existingEscort != null)
                {
                    _db.Entry(existingEscort).CurrentValues.SetValues(newEscort);
                }
                else
                {
                    newEscort.ReservationId = reservationId;
                    await _db.Escorts.AddAsync(MapEscort.MAP(newEscort));
                }
            }

            await _db.SaveChangesAsync();
        }

    }
}
