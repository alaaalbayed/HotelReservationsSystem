using Domain.DTO_s;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Ecommerce_App.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Domain.Service;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class ReservationsController : BaseController
    {
        private readonly Infrastructure.Data.Ecommerce_AppContext _db;
        private readonly UserManager<Ecommerce_AppUser> _userManager;
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IEscortService _escortService;

        public ReservationsController(
            Infrastructure.Data.Ecommerce_AppContext db,
            UserManager<Ecommerce_AppUser> userManager,
            IReservationService reservationService,
            IRoomService roomService,
            IRoomTypeService roomTypeService,
            IEscortService escortService,
            ILoggerService logger) : base(logger)
        {
            _db = db;
            _userManager = userManager;
            _reservationService = reservationService;
            _roomService = roomService;
            _roomTypeService = roomTypeService;
            _escortService = escortService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var reservations = await _reservationService.GetAllReservations();

                return View(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting reservations", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var allRoomAvailable = await _roomService.GetAllRoom();
                var model = new Reservation
                {
                    Rooms = allRoomAvailable.Select(rt => new SelectListItem
                    {
                        Value = rt.RoomId.ToString(),
                        Text = rt.RoomType.NameEn
                    }).ToList()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while preparing reservation creation", ex);
                return NotFound500();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            try
            {
                var allRoomAvailable = await _roomService.GetAllRoom();
                if (!ModelState.IsValid)
                {
                    var model = new Reservation
                    {
                        Rooms = allRoomAvailable.Select(rt => new SelectListItem
                        {
                            Value = rt.RoomId.ToString(),
                            Text = rt.RoomType.NameEn
                        }).ToList()
                    };

                    return View(model);
                }
                else
                {
                    var room = await _roomService.GetId(reservation.RoomId);

                    var userId = _userManager.GetUserId(User);
                    await _reservationService.Add(reservation, userId);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a reservation", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var reservation = await _reservationService.GetReservationById(id);
                var getCapacity = await _roomService.GetRoomCapacity(reservation.RoomId);
                var getRoomNumber = await _roomService.GetRoomNumber(reservation.RoomId);
                var getEscorts = await _escortService.GetEscorts(reservation.ReservationId ?? 0);

                ViewBag.Capacity = getCapacity;
                ViewBag.RoomNumber = getRoomNumber;
                reservation.Escorts = getEscorts;
                if (reservation == null)
                {
                    return NotFound404();
                }

                return View(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while preparing reservation editing", ex);
                return NotFound500();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            try
            {
                reservation.ReservationId = id;
                var userId = _userManager.GetUserId(User);
                reservation.UserId = userId;
                List<Escort> getEscorts = reservation.Escorts;

                if (ModelState.IsValid)
                {
                    await _reservationService.Update(id, reservation, getEscorts);
                    return RedirectToAction("Index");
                }

                return View(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating a reservation", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Info(int id)
        {
            try
            {
                var reseravtions = await _reservationService.GetReservationById(id);
                List<Escort> escort = await _escortService.GetEscorts(reseravtions.ReservationId ?? 0);

                reseravtions.Escorts = escort;

                if (reseravtions == null)
                {
                    return NotFound404();
                }

                return View(reseravtions);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting reservation info", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _reservationService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting a reservation", ex);
                return NotFound500();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoomCapacity(int roomId)
        {
            try
            {
                var roomCapacity = await _roomService.GetRoomCapacity(roomId);
                return Json(roomCapacity);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room capacity", ex);
                return NotFound500();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoomTypePrice(int roomNumber)
        {
            try
            {
                var room = await _db.Rooms.SingleOrDefaultAsync(x => x.RoomId == roomNumber);

                if (room == null)
                {
                    return NotFound404();
                }

                var roomTypeId = room.RoomTypeId;
                var roomType = await _db.RoomTypes.SingleOrDefaultAsync(x => x.TypeId == roomTypeId);

                if (roomType == null)
                {
                    return NotFound404();
                }

                var capacity = room.Capacity;
                var pricePerNight = room.PricePerNight;
                var breakfast = roomType.Breakfast;
                var lunch = roomType.Lunch;
                var dinner = roomType.Dinner;
                var extraBed = roomType.ExtraBed;

                var result = new
                {
                    PricePerNight = pricePerNight,
                    Capacity = capacity,
                    Breakfast = breakfast,
                    Lunch = lunch,
                    Dinner = dinner,
                    ExtraBed = extraBed
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room type price", ex);
                return NotFound500();
            }
        }
    }
}
