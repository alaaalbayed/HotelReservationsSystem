using Domain.DTO_s;
using Domain.MAPPER;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ISettingService
    {
        Task Add(LookUpRoomType roomType);
        Task Update(int id, LookUpRoomType roomType);
        Task Delete(int id);
        Task<int> CountAllRoomTypeAsync();
        Task<LookUpRoomType> GetRoomTypeById(int id);
        IEnumerable<LookUpRoomType> GetAllRoomTypes();
    }
}
