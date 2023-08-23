using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO_s
{
	public class Room
	{
        public int RoomId { get; set; }
        public int Capacity { get; set; }
		public double AdultPrice { get; set; }
		public double ChildrenPrice { get; set; }
		public int RoomNumber { get; set; }
        public List<IFormFile>? RoomImages { get; set; }
        public List<RoomImage> RoomImages2 { get; set; }
        public List<SelectListItem>? RoomTypes { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public long RoomTypeId { get; set; }
        public LookUpProperty? RoomType { get; set; }
    }
}
