using Domain.DTO_s;
using Microsoft.AspNetCore.Http;

namespace Domain.Interface
{
    public interface IRoomImageService
    {
        Task AddRange(int id, List<IFormFile> roomImages);
        Task RemoveAll(int roomId);
        Task Update(int roomImageId, RoomImage roomImage);
        Task<List<RoomImage>> GetAllRoomImage();

    }
}
