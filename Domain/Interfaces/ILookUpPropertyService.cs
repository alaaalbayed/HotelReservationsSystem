using dto = Domain.DTO_s;

namespace Domain.Interface
{
    public interface ILookUpPropertyService
    {
        Task Add(dto.LookUpProperty lookUpProperty);
        Task Update(int id, dto.LookUpProperty lookUpProperty);
        Task Delete(int id);
        Task<IEnumerable<dto.LookUpProperty>> GetAllLookUpProperty();
        Task<dto.LookUpProperty> GetLookUpPropertyById(int id);
        Task<dto.LookUpProperty> GetByRoomTypeId(long id);
        Task<string> GetTypeNameByRoomTypeId(long typeId);
        
    }
}
