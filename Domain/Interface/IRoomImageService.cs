using Domain.DTO_s;
using Microsoft.AspNetCore.Http;

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
