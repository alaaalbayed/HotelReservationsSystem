using Domain.DTO_s;
using Domain.Interface;
using Domain.MAPPER;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Domain.Service
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly Ecommerce_AppContext _db;

        public RoomTypeService(Ecommerce_AppContext db)
        {
            _db = db;
        }
        public async Task Add(RoomType roomType)
        {
            var _roomType = await _db.LookUpProperty
                .FirstOrDefaultAsync(lt => lt.Id == roomType.TypeId);

            if (_roomType != null)
            {
                roomType.TypeId = _roomType.Id;

                await _db.RoomTypes.AddAsync(MapRoomType.MAP(roomType));
                await _db.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var roomType = await _db.RoomTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (roomType != null)
            {
                _db.RoomTypes.Remove(roomType);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomType()
        {
            var roomTypes = await _db.RoomTypes
                .Include(x=>x.Type)
                .ToListAsync();

            return MapRoomType.MAP(roomTypes);
        }

        public Task<RoomType> GetAllRoomTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RoomType> GetRoomTypeByID(double id)
        {
            var roomType = await _db.RoomTypes.FirstOrDefaultAsync(x => x.TypeId == id);
            return MapRoomType.MAP(roomType);
        }

        public async Task<RoomType> GetById(double id)
        {
            var roomType = await _db.RoomTypes
                .Include(r => r.Type)
                .FirstOrDefaultAsync(x => x.Id == id);
            return MapRoomType.MAP(roomType);
        }

        public async Task Update(int id, RoomType roomType)
        {
            roomType.Id = id;
            var roomTypeToChange = await _db.RoomTypes.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (roomTypeToChange != null)
            {
                _db.RoomTypes.Update(MapRoomType.MAP(roomType));
                await _db.SaveChangesAsync();
            }
        }
    }
}
