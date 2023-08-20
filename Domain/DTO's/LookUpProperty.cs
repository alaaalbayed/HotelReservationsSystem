using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO_s
{
    public class LookUpProperty
    {
        public long Id { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public long TypeId { get; set; }

        public LookUpType LookUpType { get; set; }
    }
}
