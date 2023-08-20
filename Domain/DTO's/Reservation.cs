using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO_s
{
    public class Reservation
    {
        public int? ReservationId { get; set; }
        public string FullName { get; set; }
        public string Nationality { get; set; }
        public string NationalityId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsAdult { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public bool ExtraBed { get; set; }
        public double Price { get; set; }
        public int RoomId { get; set; }
        public string? UserId { get; set; }
        public List<SelectListItem>? Rooms { get; set; }
        public List<Escort> Escorts { get; set; } = new List<Escort>();

    }
}
