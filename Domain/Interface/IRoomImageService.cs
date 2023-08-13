using Domain.DTO_s;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IRoomImageService
    {
        Task AddRange(int id, List<IFormFile> roomImages);
        Task Remove(int roomImageId);
        Task Update(int roomImageId, RoomImage roomImage);
        Task<List<RoomImage>> GetAllRoomImage();

    }
}
