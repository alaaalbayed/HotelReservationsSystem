using Domain.DTO_s;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dto = Domain.DTO_s;
using orm = Infrastructure.Data;

namespace Domain.MAPPER
{
    public class MapRoom
    {
        public static orm.Rooms MAP(dto.Room obj)
        {
            var room = new orm.Rooms();
            if (obj != null)
            {
                room = new orm.Rooms
                {
                    RoomId = obj.RoomId,
                    Capacity = obj.Capacity,
                    AdultPrice = obj.AdultPrice,
                    ChildrenPrice = obj.ChildrenPrice,
                    IsTaken = obj.IsTaken,
                    RoomNumber = obj.RoomNumber,
                    RoomTypeId = obj.RoomTypeId,
                };
            }
            return room;
        }

        public static List<orm.Rooms> MAP(List<dto.Room> obj)
        {
            var list = new List<orm.Rooms>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }

        public static dto.Room MAP(orm.Rooms obj)
        {
            var room = new dto.Room();
            if (obj != null)
            {
                room = new dto.Room
                {
                    RoomId = obj.RoomId,
                    Capacity = obj.Capacity,
                    AdultPrice = obj.AdultPrice,
                    ChildrenPrice = obj.ChildrenPrice,
                    IsTaken = obj.IsTaken,
                    RoomNumber = obj.RoomNumber,
                    RoomTypeId = obj.RoomTypeId,
                    RoomType = obj.RoomType != null ? new dto.LookUpProperty
                    {
                        Id = obj.RoomType.Id,
                        NameAr = obj.RoomType.NameAr,
                        NameEn = obj.RoomType.NameEn,
                    } : null,
                    RoomImages2 = obj.RoomImages.Select(ri => new dto.RoomImage
                    {
                        RoomImageId = ri.RoomImageId,
                        ImageUrl = ri.ImageUrl
                    }).ToList()
                    
                };
            }
            return room;
        }

        public static List<dto.Room> MAP(List<orm.Rooms> obj)
        {
            var list = new List<dto.Room>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }
    }
}
