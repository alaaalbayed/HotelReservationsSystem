using Domain.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class EditLookUpPropertyViewModel
    {
        public LookUpProperty LookUpProperty { get; set; }
        public IEnumerable<LookUpType> AllLookUpTypes { get; set; }
    }
}
