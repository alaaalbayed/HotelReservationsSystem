﻿using Domain.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class EditRoomTypeViewModel
    {
        public RoomType LookUpRoomType { get; set; }
        public LookUpProperty LookUpProperty { get; set; }
    }
}
