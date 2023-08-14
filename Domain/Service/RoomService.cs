using Domain.DTO_s;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.MAPPER;
using Humanizer;
using static System.Net.Mime.MediaTypeNames;
using System.Numerics;

namespace Domain.Service
{
	public class RoomService : IRoomService
	{
        private readonly Ecommerce_AppContext _db;
        private readonly IRoomImageService _roomImageService;
        private readonly IRoomTypeService _roomTypeService;

        public RoomService(Ecommerce_AppContext db, IRoomImageService roomImageService, IRoomTypeService roomTypeService)
        {
            _db = db;
            _roomImageService = roomImageService;
            _roomTypeService = roomTypeService;
        }

        public async Task Add(Room room, List<IFormFile> roomImages)
        {
            var _room = await _db.Rooms.AddAsync(MapRoom.MAP(room));
            await _db.SaveChangesAsync();

            if (roomImages != null && roomImages.Any())
            {
                await _roomImageService.AddRange(_room.Entity.RoomId, roomImages);
            }
        }

        public async Task Update(int id, Room room)
        {
            room.RoomId = id;
            room.RoomTypeId = room.RoomTypeId;
            var roomToChange = await _db.Rooms.AsNoTracking().FirstOrDefaultAsync(x => x.RoomId == id);
            if (roomToChange != null)
            {
                _db.Rooms.Update(MapRoom.MAP(room));
                await _db.SaveChangesAsync();
            }
        }

        public async Task Update(int id, Room room, List<IFormFile> roomImages)
        {
            var selectedRoomType = await _roomTypeService.GetByTypeId(room.RoomTypeId);

            var roomToChange = await _db.Rooms.FirstOrDefaultAsync(x => x.RoomId == id);
            if (roomToChange != null)
            {
                roomToChange.RoomType.NameEn = selectedRoomType.NameEn;
                roomToChange.RoomType.NameAr = selectedRoomType.NameAr;
                roomToChange.RoomTypeId = selectedRoomType.Id;

                roomToChange.AdultPrice = room.AdultPrice;
                roomToChange.ChildrenPrice = room.ChildrenPrice;
                roomToChange.Capacity = room.Capacity;
                roomToChange.RoomNumber = room.RoomNumber;

                _db.Entry(roomToChange).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                if (roomImages != null && roomImages.Any())
                {
                    await _roomImageService.AddRange(id, roomImages);
                }
            }
        }


        public async Task Delete(int id)
        {
            var room = await _db.Rooms.FindAsync(id);

            if (room == null)
            {
                throw new ArgumentException($"Room with ID {id} not found.");
            }

            _db.Rooms.Remove(room);

            await _db.SaveChangesAsync();

        }
        public async Task<IEnumerable<Room>> GetAllRoom()
        {
            var rooms = await _db.Rooms.Include(r => r.RoomType)
                .ToListAsync();
            return rooms.Select(roomEntity => MapRoom.MAP(roomEntity));
        }
        public async Task<Room> GetId(int id)
        {
            var room = await _db.Rooms
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(r => r.RoomId == id);

            return MapRoom.MAP(room);
        }

        public async Task<bool> IsRoomNumberFree(int number, int? roomId = null)
        {
            return await _db.Rooms.AsNoTracking().Where(x => x.RoomId != roomId).AnyAsync(x => x.RoomNumber == number);
        }

        public async Task<int> GetRoomCapacity(int roomId)
        {
            var room = await _db.Rooms.FindAsync(roomId);
            if (room != null)
            {
                return room.Capacity;
            }
            return 0;
        }

        public async Task<int> GetRoomNumber(int roomId)
        {
            var room = await _db.Rooms.FindAsync(roomId);
            if (room != null)
            {
                return room.RoomNumber;
            }
            return 0;
        }

        public async Task<double> GetAdultPrice(int roomId)
        {
            var room = await _db.Rooms.FindAsync(roomId);
            if (room != null)
            {
                return room.AdultPrice;
            }
            return 0;
        }

        public async Task<double> GetChildrenPrice(int roomId)
        {
            var room = await _db.Rooms.FindAsync(roomId);
            if (room != null)
            {
                return room.ChildrenPrice;
            }
            return 0;
        }
    }
}
