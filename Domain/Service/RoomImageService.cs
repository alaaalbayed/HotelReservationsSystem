using Domain.DTO_s;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.MAPPER;
using Humanizer;

namespace Domain.Service
{
    public class RoomImageService : IRoomImageService
    {
        private readonly Ecommerce_AppContext _db;
        private readonly ILoggerService _logger;

        public RoomImageService(Ecommerce_AppContext db, ILoggerService logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task AddRange(int roomId, List<IFormFile> roomImages)
        {
            if (roomImages != null && roomImages.Any())
            {
                foreach (var imageFile in roomImages)
                {
                    string imageUrl = await UploadImage(imageFile);

                    var newImageDto = new RoomImage
                    {
                        ImageUrl = imageUrl,
                        RoomId = roomId
                    };

                    var newImageEntity = MapImage.MAP(newImageDto);
                    _db.RoomImages.Add(newImageEntity); 
                }

                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveAll(int roomId)
        {
            var roomImages = await _db.RoomImages.Where(r => r.RoomId == roomId).ToListAsync();

            if (roomImages != null && roomImages.Any())
            {
                _db.RoomImages.RemoveRange(roomImages);
                await _db.SaveChangesAsync();
            }
        }


        public async Task Update(int Id, RoomImage roomImage)
        {
            roomImage.RoomImageId = Id;
            var updateRoomImage = await _db.RoomImages.FirstOrDefaultAsync(u => u.RoomImageId == Id);

            if (updateRoomImage != null)
            {
                updateRoomImage.ImageUrl = roomImage.ImageUrl;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<RoomImage>> GetAllRoomImage()
        {
            var allRoomImage = await _db.RoomImages.ToListAsync();
            return MapImage.MAP(allRoomImage);
        }

        private async Task<string> UploadImage(IFormFile imageFile)
        {
            try
            {
                string imageName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";

                string imagePath = Path.Combine(@"wwwroot/Images", imageName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                string imageUrl = $"/Images/{imageName}"; 
                return imageUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error uploading image: {Message}", ex);
                return "DefaultImage.jpg"; 
            }
        }
    }
}
