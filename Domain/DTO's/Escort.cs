using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO_s
{
    public class Escort
    {
        public int? EscortId { get; set; }
        public string FullName { get; set; }
        public bool IsAdult { get; set; }
        public int? ReservationId { get; set; }
    }
}
