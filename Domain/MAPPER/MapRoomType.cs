using dto = Domain.DTO_s;
using orm = Infrastructure.Data;
namespace Domain.MAPPER
{
    public class MapRoomType
    {
        public static dto.RoomType MAP(orm.RoomTypes obj)
        {
            var roomType = new dto.RoomType();

            if (obj != null)
            {
                roomType = new dto.RoomType
                {
                    Id = obj.Id,
                    Breakfast = obj.Breakfast,
                    Dinner = obj.Dinner,
                    Lunch = obj.Lunch,
                    ExtraBed = obj.ExtraBed,
                    TypeId = obj.TypeId,
                    Type = new dto.LookUpProperty
                    {
                        Id = obj.Id,
                        NameEn = obj.Type.NameEn,
                        NameAr = obj.Type.NameAr,
                        TypeId = obj.Type.TypeId
                    }
                };
            }
            return roomType;
        }

        public static List<dto.RoomType> MAP(List<orm.RoomTypes> in_obj)
        {
            var list = new List<dto.RoomType>();

            if (in_obj != null)
            {
                foreach (var item in in_obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }

        public static orm.RoomTypes MAP(dto.RoomType obj)
        {
            var roomType = new orm.RoomTypes();

            if (obj != null)
            {
                roomType = new orm.RoomTypes
                {
                    Id = obj.Id,
                    Breakfast = obj.Breakfast,
                    Dinner = obj.Dinner,
                    Lunch = obj.Lunch,
                    ExtraBed = obj.ExtraBed,
                    TypeId = obj.TypeId,
                };
            }
            return roomType;
        }

        public static List<orm.RoomTypes> MAP(List<dto.RoomType> obj)
        {
            var list = new List<orm.RoomTypes>();

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
