using Domain.DTO_s;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Domain.MAPPER;
using Domain.Service;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Ecommerce_App.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin")]
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
            ILoggerService logger
            ) : base(logger)
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
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
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
                        Text = rt.RoomNumber.ToString()
                    }).ToList()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while preparing reservation creation", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
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
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a reservation", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
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
                    return NotFound();
                }

                return View(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while preparing reservation editing", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
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

                if (id == null)
                {
                    return NotFound();
                }

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
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
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
                    return NotFound();
                }

                return View(reseravtions);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting reservation info", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                await _reservationService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting a reservation", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpGet]
        public IActionResult GetRoomCapacity(int roomId)
        {
            try
            {
                var roomCapacity = _roomService.GetRoomCapacity(roomId);
                return Json(roomCapacity);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room capacity", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
