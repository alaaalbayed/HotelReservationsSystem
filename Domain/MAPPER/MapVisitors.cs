using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dto = Domain.DTO_s;
using orm = Infrastructure.Data;
namespace Domain.MAPPER
{
    public class MapVisitors
    {
        public static dto.Visitors MAP(orm.Visitors obj)
        {
            var visitor = new dto.Visitors();
            if (obj != null)
            {
                visitor = new dto.Visitors()
                {
                    VisitorId = obj.VisitorId,
                    Ipaddress = obj.Ipaddress,
                    VisitDate = obj.VisitDate
                };
            }
            return visitor;
        }

        public static List<dto.Visitors> MAP(List<orm.Visitors> obj)
        {
            var list = new List<dto.Visitors>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }

        public static orm.Visitors MAP(dto.Visitors obj)
        {
            var visitor = new orm.Visitors();
            if (obj != null)
            {
                visitor = new orm.Visitors()
                {
                    VisitorId = obj.VisitorId,
                    Ipaddress = obj.Ipaddress,
                    VisitDate = obj.VisitDate
                };
            }
            return visitor;
        }

        public static List<orm.Visitors> MAP(List<dto.Visitors> obj)
        {
            var list = new List<orm.Visitors>();
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
