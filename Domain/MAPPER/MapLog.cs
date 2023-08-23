using dto = Domain.DTO_s;
using orm = Infrastructure.Data;

namespace Domain.MAPPER
{
    public class MapLog
    {
        public static orm.Logs MAP(dto.Log obj)
        {
            var log = new orm.Logs();

            if (obj != null)
            {
                log = new orm.Logs
                {
                    Id = obj.Id,
                    Level = obj.Level,
                    Class = obj.Class,
                    Method = obj.Method,
                    Exception = obj.Exception,
                    Message = obj.Message,
                    Timestamp = obj.Timestamp
                };
            }
            return log;
        }

        public static List<orm.Logs> MAP(List<dto.Log> obj)
        {
            var list = new List<orm.Logs>();

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
