using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dto = Domain.DTO_s;
using orm = Infrastructure.Data;
namespace Domain.MAPPER
{
    public class MapReservation
    {
        public static dto.Reservation MAP(orm.Reservations obj)
        {
            var reservation = new dto.Reservation();
            if (obj != null)
            {
                reservation = new dto.Reservation()
                {

                    ReservationId = obj.ReservationId,
                    FullName = obj.FullName,
                    Nationality = obj.Nationality,
                    NationalityId = obj.NationalityId,
                    PhoneNumber = obj.PhoneNumber,
                    Email = obj.Email,
                    IsAdult = obj.IsAdult,
                    CheckIn = obj.CheckIn,
                    CheckOut = obj.CheckOut,
                    Breakfast = obj.Breakfast,
                    Lunch = obj.Lunch,
                    Dinner = obj.Dinner,
                    ExtraBed = obj.ExtraBed,
                    Price = obj.Price,
                    RoomId = obj.RoomId,
                    UserId = obj.UserId

                };
            }
            return reservation;
        }

        public static List<dto.Reservation> MAP(List<orm.Reservations> obj)
        {
            var list = new List<dto.Reservation>();

            if (obj != null)
            {
                foreach (var item in obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }

        public static orm.Reservations MAP(dto.Reservation obj)
        {
            var reservation = new orm.Reservations();
            if (obj != null)
            {
                reservation = new orm.Reservations()
                {
                    ReservationId = obj.ReservationId ?? 0,
                    FullName = obj.FullName,
                    Nationality = obj.Nationality,
                    NationalityId = obj.NationalityId,
                    PhoneNumber = obj.PhoneNumber,
                    Email = obj.Email,
                    IsAdult = obj.IsAdult,
                    CheckIn = obj.CheckIn,
                    CheckOut = obj.CheckOut,
                    Breakfast = obj.Breakfast,
                    Lunch = obj.Lunch,
                    Dinner = obj.Dinner,
                    ExtraBed = obj.ExtraBed,
                    Price = obj.Price,
                    RoomId = obj.RoomId,
                    UserId = obj.UserId,
                };
            }
            return reservation;
        }

        public static List<orm.Reservations> MAP(List<dto.Reservation> obj)
        {
            var list = new List<orm.Reservations>();

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
