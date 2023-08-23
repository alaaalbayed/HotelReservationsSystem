using dto = Domain.DTO_s;
using orm = Infrastructure.Data;

namespace Domain.MAPPER
{
    public class MapLookUpType
    {
        public static dto.LookUpType MAP(orm.LookUpType obj)
        {
            var roomType = new dto.LookUpType();

            if (obj != null)
            {
                roomType = new dto.LookUpType
                {
                    Id = obj.Id,
                    NameAr = obj.NameAr,
                    NameEn = obj.NameEn,
                };
            }
            return roomType;
        }

        public static List<dto.LookUpType> MAP(List<orm.LookUpType> in_obj)
        {
            var list = new List<dto.LookUpType>();

            if (in_obj != null)
            {
                foreach (var item in in_obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }

        public static orm.LookUpType MAP(dto.LookUpType obj)
        {
            var roomType = new orm.LookUpType();

            if (obj != null)
            {
                roomType = new orm.LookUpType
                {
                    Id = obj.Id,
                    NameAr = obj.NameAr,
                    NameEn = obj.NameEn,
                };
            }
            return roomType;
        }

        public static List<orm.LookUpType> MAP(List<dto.LookUpType> in_obj)
        {
            var list = new List<orm.LookUpType>();

            if (in_obj != null)
            {
                foreach (var item in in_obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }
    }
}
