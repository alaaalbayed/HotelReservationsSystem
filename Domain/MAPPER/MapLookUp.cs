using Humanizer;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dto = Domain.DTO_s;
using orm = Infrastructure.Data;

namespace Domain.MAPPER
{
    public class MapLookUp
    {
        public static dto.LookUpProperty MAP(orm.LookupProperty obj)
        {
            var roomType = new dto.LookUpProperty();

            if (obj != null)
            {
                roomType = new dto.LookUpProperty
                {
                    Id = obj.Id,
                    TypeId = obj.TypeId,
                    NameAr = obj.NameAr,
                    NameEn = obj.NameEn,
                };
            }
            return roomType;
        }

        public static List<dto.LookUpProperty> MAP(List<orm.LookupProperty> in_obj)
        {
            var list = new List<dto.LookUpProperty>();

            if (in_obj != null)
            {
                foreach (var item in in_obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }

        public static orm.LookupProperty MAP(dto.LookUpProperty obj)
        {
            var roomType = new orm.LookupProperty();

            if (obj != null)
            {
                roomType = new orm.LookupProperty
                {
                    TypeId = obj.TypeId,
                    NameAr = obj.NameAr,
                    NameEn = obj.NameEn,
                };
            }
            return roomType;
        }

        public static List<orm.LookupProperty> MAP(List<dto.LookUpProperty> in_obj)
        {
            var list = new List<orm.LookupProperty>();

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

