using Domain.DTO_s;
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
    public class MapLookUpProperty
    {
        public static dto.LookUpProperty MAP(orm.LookUpProperty obj)
        {
            var roomType = new dto.LookUpProperty();

            if (obj != null)
            {
                roomType = new dto.LookUpProperty
                {
                    Id = obj.Id,
                    NameAr = obj.NameAr,
                    NameEn = obj.NameEn,
                    TypeId = obj.TypeId,

                };
            }
            return roomType;
        }

        public static List<dto.LookUpProperty> MAP(List<orm.LookUpProperty> in_obj)
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

        public static orm.LookUpProperty MAP(dto.LookUpProperty obj)
        {
            var roomType = new orm.LookUpProperty();

            if (obj != null)
            {
                roomType = new orm.LookUpProperty
                {
                    Id = obj.Id,
                    NameAr = obj.NameAr,
                    NameEn = obj.NameEn,
                    TypeId = obj.TypeId,
                };
            }
            return roomType;
        }

        public static List<orm.LookUpProperty> MAP(List<dto.LookUpProperty> in_obj)
        {
            var list = new List<orm.LookUpProperty>();

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

