using Domain.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CalculateFinalPriceRequest
    {
        public Reservation Reservation { get; set; }
        public List<Escort> Escorts { get; set; }
    }
}
