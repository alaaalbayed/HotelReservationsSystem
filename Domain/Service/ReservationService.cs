using Domain.DTO_s;
using Domain.Interface;
using Domain.MAPPER;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Domain.Service
{
    public class ReservationService : IReservationService
    {
        private readonly Ecommerce_AppContext _db;
        private readonly IEscortService _ecortService;
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;

        public ReservationService(
            Ecommerce_AppContext db,
            IEscortService ecortService,
            IRoomService roomService,
            IRoomTypeService roomTypeService
            )
        {
            _db = db;
            _ecortService = ecortService;
            _roomService = roomService;
            _roomTypeService = roomTypeService;

        }

        public async Task Add(Reservation reservation, string userId)
        {

            reservation.UserId = userId;
            reservation.OrderDate = DateTime.Now;
            List<Escort> escorts = reservation.Escorts.ToList();

            double finalPrice = await GetFinalPrice(reservation, escorts);
            reservation.Price = finalPrice;

            var _reservation = await _db.Reservations.AddAsync(MapReservation.MAP(reservation));
            await _db.SaveChangesAsync();

            if (escorts != null && escorts.Any())
            {
                await _ecortService.Add(_reservation.Entity.ReservationId, escorts);
            }

        }

        public async Task Update(int reservationId, Reservation updatedReservation, List<Escort> escorts)
        {
            var reservation = await _db.Reservations.FindAsync(reservationId);
            if (reservation != null)
            {
                reservation.Breakfast = updatedReservation.Breakfast;
                reservation.CheckIn = updatedReservation.CheckIn;
                reservation.CheckOut = updatedReservation.CheckOut;
                reservation.Dinner = updatedReservation.Dinner;
                reservation.Email = updatedReservation.Email;
                reservation.ExtraBed = updatedReservation.ExtraBed;
                reservation.FullName = updatedReservation.FullName;
                reservation.IsAdult = updatedReservation.IsAdult;
                reservation.Lunch = updatedReservation.Lunch;
                reservation.Nationality = updatedReservation.Nationality;
                reservation.NationalityId = updatedReservation.NationalityId;
                reservation.PhoneNumber = updatedReservation.PhoneNumber;
                reservation.Price = updatedReservation.Price;

                double finalPrice = await GetFinalPrice(updatedReservation, escorts);
                reservation.Price = finalPrice;

                await _ecortService.UpdateEscorts(reservationId, escorts);

                _db.Entry(reservation).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }

        public async Task Delete(int reservationId)
        {
            var reservation = await _db.Reservations.FindAsync(reservationId);
            if (reservation != null)
            {
                _db.Reservations.Remove(reservation);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Reservation> GetReservationById(int reservationId)
        {
            var reservations = await _db.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

            return MapReservation.MAP(reservations);
        }

        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            var reservations = await _db.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .OrderByDescending(r => r.OrderDate)
                .ToListAsync();

            return reservations.Select(r => MapReservation.MAP(r));
        }

        public async Task<bool> AreDatesAcceptable(int roomId, DateTime CheckIn, DateTime CheckOut, int? reservationId)
        {
            if (CheckIn >= CheckOut || CheckIn < DateTime.Today)
            {
                return false;
            }

            var reservationPeriods = await _db.
                                           Reservations.
                                           AsNoTracking().
                                           Where(x => x.Room.RoomId == roomId).
                                           Select(x => new Tuple<DateTime, DateTime>
                                                        (x.CheckIn, x.CheckOut).
                                                        ToValueTuple()).
                                          ToListAsync();

            if (reservationId > 0)
            {
                var reservation = await _db.Reservations.AsNoTracking().FirstOrDefaultAsync(x => x.ReservationId == reservationId);
                reservationPeriods = reservationPeriods.Where(x => x.Item1 != reservation.CheckIn &&
                                                              x.Item2 != reservation.CheckOut).ToList();
            }

            return !reservationPeriods.Any(x =>
                (x.Item1 >= CheckIn && x.Item1 <= CheckOut) ||
                (x.Item2 > CheckIn && x.Item2 <= CheckOut) ||
                (x.Item1 >= CheckIn && x.Item2 <= CheckOut) ||
                (x.Item1 <= CheckIn && x.Item2 >= CheckOut));
        }

        public int GetDaysBetweenDates(DateTime chickIn, DateTime chickOut)
        {
            TimeSpan timeSpan = chickOut.Date - chickIn.Date;
            int days = timeSpan.Days;

            return days + 1;
        }

        public async Task<double> GetFinalPrice(Reservation reservation, List<Escort> escorts)
        {
            double price = 0;
            double roomPrice = reservation.IsAdult ? await _roomService.GetAdultPrice(reservation.RoomId) : await _roomService.GetChildrenPrice(reservation.RoomId);
            var room = await _roomService.GetRoomByRoomId(reservation.RoomId);
            var roomType = await _roomTypeService.GetRoomTypeByID(room.RoomTypeId);

            if (reservation.Lunch)
                price += roomType.Lunch;

            if (reservation.Dinner)
                price += roomType.Dinner;

            if (reservation.Breakfast)
                price += roomType.Breakfast;

            if (reservation.ExtraBed)
                price += roomType.ExtraBed;

            foreach (Escort escort in escorts)
            {
                double escortPrice = escort.IsAdult ? await _roomService.GetAdultPrice(reservation.RoomId) : await _roomService.GetChildrenPrice(reservation.RoomId);
                price += escortPrice;
            }

            int numberOfDays = GetDaysBetweenDates(reservation.CheckIn, reservation.CheckOut);
            return (price + roomPrice) * numberOfDays;
        }
    }
}
