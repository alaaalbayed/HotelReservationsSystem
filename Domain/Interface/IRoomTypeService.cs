using Domain.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
	public interface IRoomTypeService
	{
        Task Add(LookUpProperty roomType);
        Task Update(int id, LookUpProperty roomType);
        Task Delete(int id);
        Task<LookUpProperty> GetRoomTypeById(int id);
        Task<LookUpProperty> GetByTypeId(int id);
        IEnumerable<LookUpProperty> GetAllRoomTypes();
    }
}
