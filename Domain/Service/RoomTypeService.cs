using Domain.DTO_s;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.MAPPER;

namespace Domain.Service
{
	public class RoomTypeService : IRoomTypeService
	{
        private readonly Ecommerce_AppContext _db;

        public RoomTypeService(Ecommerce_AppContext db)
        {
            _db = db;
        }

        public async Task Add(LookUpProperty roomType)
        {
            var selectedRoomType = await _db.LookupRoomType.FindAsync((long)roomType.Id);

            if (selectedRoomType == null)
            {
                throw new ArgumentException("Invalid selected room type ID.");
            }

            var lookupProperty = MapLookUp.MAP(roomType);
            lookupProperty.TypeId = selectedRoomType.Id;

            await _db.LookupProperty.AddAsync(lookupProperty);
            await _db.SaveChangesAsync();
        }


        public async Task Update(int id, LookUpProperty roomType)
        {
            var roomTypeToUpdate = await _db.LookupProperty.FindAsync(id);
            if (roomTypeToUpdate != null)
            {
                roomTypeToUpdate.NameEn = roomType.NameEn;
                roomTypeToUpdate.NameAr = roomType.NameAr;

                await _db.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var data = await _db.LookupProperty.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                _db.LookupProperty.Remove(data);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<int> CountAllRoomTypeAsync()
        {
            return await _db.LookupProperty.CountAsync();
        }

        public async Task<LookUpProperty> GetRoomTypeById(int id)
        {
            var roomType = await _db.LookupProperty.FirstOrDefaultAsync(x => x.Id == id);
            return MapLookUp.MAP(roomType);
        }

        public async Task<LookUpProperty> GetByTypeId(int id)
        {
            var roomType = await _db.LookupProperty.FirstOrDefaultAsync(x => x.TypeId == id);
            return MapLookUp.MAP(roomType);
        }

        public async Task<IEnumerable<LookUpProperty>> GetAllRoomTypes()
        {
            var roomTypes = await _db.LookupProperty.ToListAsync();
            return MapLookUp.MAP(roomTypes);
        }
    }
}
