using Domain.Interface;
using Domain.MAPPER;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using dto = Domain.DTO_s;

namespace Domain.Service
{
    public class LookUpTypeService : ILookUpTypeService
    {
        private readonly Ecommerce_AppContext _db;

        public LookUpTypeService(Ecommerce_AppContext db)
        {
            _db = db;
        }

        public async Task Add(dto.LookUpType lookUpType)
        {
            await _db.LookUpType.AddAsync(MapLookUpType.MAP(lookUpType));
            await _db.SaveChangesAsync();
        }

        public async Task Update(int id, dto.LookUpType lookUpType)
        {
            lookUpType.Id = id;
            var roomTypeToChange = await _db.LookUpType.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (roomTypeToChange != null)
            {
                _db.LookUpType.Update(MapLookUpType.MAP(lookUpType));
                await _db.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var data = await _db.LookUpType.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                _db.LookUpType.Remove(data);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<DTO_s.LookUpType> GetLookUpTypeById(int id)
        {
            var lookUpType = await _db.LookUpType.FirstOrDefaultAsync(x => x.Id == id);
            return MapLookUpType.MAP(lookUpType);
        }

        public async Task<IEnumerable<dto.LookUpType>> GetAllLookUpTypes()
        {
            var allLookUpTypes = await _db.LookUpType.ToListAsync();
            return MapLookUpType.MAP(allLookUpTypes);
        }

    }
}
