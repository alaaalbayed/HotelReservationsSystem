using Domain.Interface;
using Domain.MAPPER;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using dto = Domain.DTO_s;

namespace Domain.Service
{
    public class LookUpPropertyService : ILookUpPropertyService
    {
        private readonly Ecommerce_AppContext _db;
        public LookUpPropertyService(Ecommerce_AppContext db)
        {
            _db = db;
        }
        public async Task Add(dto.LookUpProperty lookUpProperty)
        {
            var lookUpType = await _db.LookUpType
            .FirstOrDefaultAsync(lt => lt.Id == lookUpProperty.TypeId);

            if (lookUpType != null)
            {
                lookUpProperty.TypeId = lookUpType.Id;

                await _db.LookUpProperty.AddAsync(MapLookUpProperty.MAP(lookUpProperty));
                await _db.SaveChangesAsync();
            }
        }

        public async Task Update(int id, dto.LookUpProperty lookUpProperty)
        {
            lookUpProperty.Id = id;
            var _lookUpProperty = await _db.LookUpProperty.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (_lookUpProperty != null)
            {
                _db.LookUpProperty.Update(MapLookUpProperty.MAP(lookUpProperty));
                await _db.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var lookUpProperty = await _db.LookUpProperty.FirstOrDefaultAsync(x => x.Id == id);
            if (lookUpProperty != null)
            {
                _db.LookUpProperty.Remove(lookUpProperty);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<dto.LookUpProperty>> GetAllLookUpProperty()
        {
            var allLookUpProperty = await _db.LookUpProperty
                .Include(prop => prop.Type)
                .ToListAsync();
            return MapLookUpProperty.MAP(allLookUpProperty);
        }

        public async Task<dto.LookUpProperty> GetLookUpPropertyById(int id)
        {
            var lookUpType = await _db.LookUpProperty
                .Include(prop => prop.Type)
                .FirstOrDefaultAsync(x => x.Id == id);
            return MapLookUpProperty.MAP(lookUpType);
        }

        public async Task<dto.LookUpProperty> GetByRoomTypeId(long id)
        {
            var roomType = await _db.LookUpProperty.FirstOrDefaultAsync(x => x.Id == id);
            return MapLookUpProperty.MAP(roomType);
        }

        public async Task<string> GetTypeNameByRoomTypeId(long typeId)
        {
            var check = CultureInfo.CurrentCulture.Name;
            var lookUpProperty = await _db.LookUpType.FirstOrDefaultAsync(l => l.Id == typeId);
            if (lookUpProperty != null)
            {
                if (check == "en-US")
                {
                    return lookUpProperty.NameEn;
                }
                else
                {
                    return lookUpProperty.NameAr;
                }
            }
            return null;
        }
    }
}
