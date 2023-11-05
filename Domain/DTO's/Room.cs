using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.DTO_s
{
    public class Room
	{
        public int RoomId { get; set; }
        public int Capacity { get; set; }
		public int RoomNumber { get; set; }
        public bool Status { get; set; }
        public double PricePerNight { get; set; }
        public List<IFormFile>? RoomImages { get; set; }
        public List<RoomImage> RoomImages2 { get; set; }
        public List<SelectListItem>? RoomTypes { get; set; }
        public bool KeepExistingImages { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public long RoomTypeId { get; set; }
        public LookUpProperty? RoomType { get; set; }

    }
}
