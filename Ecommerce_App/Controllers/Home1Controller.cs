using Domain.DTO_s;
using Domain.Interface;
using Domain.Service;
using Ecommerce_App.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin, Employee, User")]
    public class Home1Controller : Controller
    {
        private readonly Infrastructure.Data.Ecommerce_AppContext _db;
        private readonly UserManager<Ecommerce_AppUser> _userManager;
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;

        public Home1Controller(Infrastructure.Data.Ecommerce_AppContext db, UserManager<Ecommerce_AppUser> userManager, IReservationService reservationService, IRoomService roomService)
        {
            _db = db;
            _userManager = userManager;
            _reservationService = reservationService;
            _roomService = roomService;
        }
        public async Task<IActionResult> Index()
        {
            var allRoomAvailable = await _roomService.GetAllRoom();
            var model = new Reservation
            {
                Rooms = allRoomAvailable.Select(rt => new SelectListItem
                {
                    Value = rt.RoomId.ToString(),
                    Text = rt.RoomNumber.ToString()
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Reservation reservation)
        {
            var allRoomAvailable = await _roomService.GetAllRoom();
            if (!ModelState.IsValid)
            {
                var model = new Reservation
                {
                    Rooms = allRoomAvailable.Select(rt => new SelectListItem
                    {
                        Value = rt.RoomId.ToString(),
                        Text = rt.RoomNumber.ToString()
                    }).ToList()
                };

                return View(model);
            }
            else
            {
                var room = await _roomService.GetId(reservation.RoomId);

                var roomIsEmpty = await _reservationService.AreDatesAcceptable(room.RoomId,
                                                                              reservation.CheckIn,
                                                                              reservation.CheckOut,
                                                                              null);
                if (!roomIsEmpty)
                {
                    ModelState.AddModelError(nameof(reservation.CheckIn), "Room is already reserved at that time");
                    var model = new Reservation
                    {
                        Rooms = allRoomAvailable.Select(rt => new SelectListItem
                        {
                            Value = rt.RoomId.ToString(),
                            Text = rt.RoomNumber.ToString()
                        }).ToList()
                    };
                    return View(model);
                }

                var userId = _userManager.GetUserId(User);
                await _reservationService.Add(reservation, userId);

                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetRoomCapacity(int roomId)
        {
            var roomCapacity = await _roomService.GetRoomCapacity(roomId);
            return Json(roomCapacity);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoomTypePrice(int roomNumber)
        {
            var room = await _db.Rooms.SingleOrDefaultAsync(x => x.RoomId == roomNumber);


            var roomTypeId = room.RoomTypeId;
            var roomType = await _db.RoomTypes.SingleOrDefaultAsync(x => x.TypeId == roomTypeId);



            var adultPrice = room.AdultPrice;
            var childrenPrice = room.ChildrenPrice;
            var breakfast = roomType.Breakfast;
            var lunch = roomType.Lunch;
            var dinner = roomType.Dinner;
            var extraBed = roomType.ExtraBed;

            var result = new
            {
                AdultPrice = adultPrice,
                ChildrenPrice = childrenPrice,
                Breakfast = breakfast,
                Lunch = lunch,
                Dinner = dinner,
                ExtraBed = extraBed
            };

            return Json(result);
        }
    }
}


