using dto = Domain.DTO_s;
using orm = Infrastructure.Data;

namespace Domain.MAPPER
{
    public class MapEscort
    {
        public static dto.Escort MAP(orm.Escorts obj)
        {
            var escort = new dto.Escort();
            if (obj != null)
            {
                escort = new dto.Escort()
                {
                    FullName = obj.FullName,
                    IsAdult = obj.IsAdult,
                    ReservationId = obj.ReservationId
                };
            }
            return escort;
        }

        public static List<dto.Escort> MAP(List<orm.Escorts> obj)
        {
            var list = new List<dto.Escort>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }

        public static orm.Escorts MAP(dto.Escort obj)
        {
            var escort = new orm.Escorts();
            if (obj != null)
            {
                escort = new orm.Escorts()
                {
                    FullName = obj.FullName,
                    IsAdult = obj.IsAdult,
                    ReservationId = obj.ReservationId ?? 0
                };
            }
            return escort;
        }

        public static List<orm.Escorts> MAP(List<dto.Escort> obj)
        {
            var list = new List<orm.Escorts>();
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
