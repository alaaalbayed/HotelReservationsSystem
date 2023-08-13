using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO_s
{
    public class RoomImage
    {
        public int RoomImageId { get; set; }
        public string ImageUrl { get; set; }
        public int RoomId { get; set; }

    }
}
