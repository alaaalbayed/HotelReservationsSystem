using Domain.DTO_s;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class LookUpPropertyIndexView
    {
        public List<SelectListItem>? RoomTypes { get; set; }
        public List<Room>? Rooms { get; set; }

    }
}
