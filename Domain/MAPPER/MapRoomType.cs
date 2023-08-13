using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using dto = Domain.DTO_s;
using orm = Infrastructure.Data;
namespace Domain.MAPPER
{
    public class MapRoomType
    {
        public static dto.LookUpRoomType MAP(orm.LookupRoomType obj)
        {
            var roomType = new dto.LookUpRoomType();

            if (obj != null)
            {
                roomType = new dto.LookUpRoomType
                {
                    Id = obj.Id,
                    NameAr = obj.NameAr,
                    NameEn = obj.NameEn,
                };
            }
            return roomType;
        }

        public static List<dto.LookUpRoomType> MAP(List<orm.LookupRoomType> in_obj)
        {
            var list = new List<dto.LookUpRoomType>();

            if (in_obj != null)
            {
                foreach (var item in in_obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }

        public static orm.LookupRoomType MAP(dto.LookUpRoomType obj)
        {
            var roomType = new orm.LookupRoomType();

            if (obj != null)
            {
                roomType = new orm.LookupRoomType
                {
                    Id = obj.Id,
                    NameAr = obj.NameAr,
                    NameEn = obj.NameEn,
                };
            }
            return roomType;
        }

        public static List<orm.LookupRoomType> MAP(List<dto.LookUpRoomType> obj)
        {
            var list = new List<orm.LookupRoomType>();

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
