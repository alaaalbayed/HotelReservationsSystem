using Domain.DTO_s;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Domain.MAPPER;
using System.Collections;

namespace Domain.Service
{
	public class RoomService : IRoomService
	{
        private readonly Ecommerce_AppContext _db;
        private readonly IRoomImageService _roomImageService;
        private readonly ILookUpTypeService _roomTypeService;
        private readonly ILookUpPropertyService _lookUpPropertyService;

        public RoomService(Ecommerce_AppContext db, IRoomImageService roomImageService, ILookUpTypeService roomTypeService, ILookUpPropertyService lookUpPropertyService)
        {
            _db = db;
            _roomImageService = roomImageService;
            _roomTypeService = roomTypeService;
            _lookUpPropertyService = lookUpPropertyService;
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
            var selectedRoomType = await _lookUpPropertyService.GetByRoomTypeId(room.RoomTypeId);

            var roomToChange = await _db.Rooms.FirstOrDefaultAsync(x => x.RoomId == id);
            if (roomToChange != null)
            {
                roomToChange.RoomTypeId = selectedRoomType.Id;

                roomToChange.PricePerNight = room.PricePerNight;
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
            if (room != null)
            {
                room.Status = !room.Status;
                _db.Rooms.Update(room);
                await _db.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<Room>> GetAllRoom()
        {
            var rooms = await _db.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .ToListAsync();

            return rooms.Select(roomEntity =>
            {
                var roomDto = MapRoom.MAP(roomEntity);

                var roomImages = roomEntity.RoomImages.Select(imageEntity => new RoomImage
                {
                    ImageUrl = imageEntity.ImageUrl

                }).ToList();

                roomDto.RoomImages2 = roomImages;

                return roomDto;
            });
        }


        public async Task<IEnumerable<Room>> GetAllActiveRooms()
        {
            var rooms = await _db.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .Where(r => r.Status == false)
                .ToListAsync();

            return rooms.Select(roomEntity =>
            {
                var roomDto = MapRoom.MAP(roomEntity);

                var roomImages = roomEntity.RoomImages.Select(imageEntity => new RoomImage
                {
                    ImageUrl = imageEntity.ImageUrl

                }).ToList();

                roomDto.RoomImages2 = roomImages;

                return roomDto;
            });
        }



        public async Task<Room> GetId(int id)
        {
            var room = await _db.Rooms
                .Include(r => r.RoomImages)
                .Include(r => r.RoomType)
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

        public async Task<double> GetPricePerNight(int roomId)
        {
            var room = await _db.Rooms.FindAsync(roomId);
            if (room != null)
            {
                return room.PricePerNight;
            }
            return 0;
        }
        public async Task<Room> GetRoomByRoomId(int id)
        {
            var room = await _db.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(x => x.RoomId == id);
                
            return MapRoom.MAP(room);
        }

    }
}
