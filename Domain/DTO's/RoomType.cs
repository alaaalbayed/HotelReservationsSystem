using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO_s
{
    public class RoomType
    {
        public long Id { get; set; }

        public double Breakfast { get; set; }

        public double Dinner { get; set; }

        public double Lunch { get; set; }

        public double ExtraBed { get; set; }

        public long TypeId { get; set; }

        public LookUpProperty? Type { get; set; }
    }
}
