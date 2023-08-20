using Domain.DTO_s;
using Domain.MAPPER;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dto = Domain.DTO_s;
namespace Domain.Interface
{
    public interface ILookUpTypeService
    {
        Task Add(dto.LookUpType roomType);
        Task Update(int id, dto.LookUpType roomType);
        Task Delete(int id);
        Task<dto.LookUpType> GetLookUpTypeById(int id);
        Task<IEnumerable<dto.LookUpType>> GetAllLookUpTypes();
    }
}
