using Domain.DTO_s;
using Microsoft.AspNetCore.Http;

namespace Domain.Interface
{
    public interface IRoomService
	{
		Task Add(Room room, List<IFormFile> roomImages);
		Task Update(int id, Room room, List<IFormFile> roomImages);
		Task Update(int id, Room room);
		Task Delete(int id);
		Task<IEnumerable<Room>> GetAllRoom();
        Task<IEnumerable<Room>> GetAllActiveRooms();
        Task<Room> GetId(int id);
        Task<bool> IsRoomNumberFree(int number, int? roomId = null);
		Task<int> GetRoomCapacity(int roomId);
		Task<int> GetRoomNumber(int roomId);
		Task<double> GetPricePerNight(int roomId);
        Task<Room> GetRoomByRoomId(int id);

    }
}
