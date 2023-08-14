using Domain.DTO_s;
using Domain.Interface;
using Domain.MAPPER;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class SettingService : ISettingService
    {
        private readonly Ecommerce_AppContext _db;

        public SettingService(Ecommerce_AppContext db)
        {
            _db = db;
        }

        public async Task Add(LookUpRoomType roomType)
        {
            await _db.LookupRoomType.AddAsync(MapRoomType.MAP(roomType));
            await _db.SaveChangesAsync();
        }

        public async Task Update(int id, LookUpRoomType roomType)
        {
            roomType.Id = id;
            var roomTypeToChange = await _db.LookupRoomType.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (roomTypeToChange != null)
            {
                _db.LookupRoomType.Update(MapRoomType.MAP(roomType));
                await _db.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var data = await _db.LookupRoomType.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                _db.LookupRoomType.Remove(data);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<int> CountAllRoomTypeAsync()
        {
            return await _db.LookupRoomType.CountAsync();
        }

        public async Task<LookUpRoomType> GetRoomTypeById(int id)
        {
            var roomType = await _db.LookupRoomType.FirstOrDefaultAsync(x => x.Id == id);
            return MapRoomType.MAP(roomType);
        }

        public async Task<IEnumerable<LookUpRoomType>> GetAllRoomTypes()
        {
            var roomTypes = await _db.LookupRoomType.ToListAsync();
            return MapRoomType.MAP(roomTypes);
        }
    }
}
