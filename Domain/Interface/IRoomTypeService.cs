﻿using Domain.DTO_s;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dto = Domain.DTO_s;

namespace Domain.Interface
{
    public interface IRoomTypeService
    {
        Task Add(dto.RoomType roomType);
        Task Update(int id, dto.RoomType roomType);
        Task Delete(int id);
        Task<IEnumerable<dto.RoomType>> GetAllRoomType();
        Task<dto.RoomType> GetAllRoomTypeById(int id);
        Task<dto.RoomType> GetRoomTypeByID(double id);
    }
}
