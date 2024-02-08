using Domain.DTO_s;
using Domain.Interface;
using Domain.Models;
using Domain.Service;
using Ecommerce_App.Areas.Identity.Data;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly Infrastructure.Data.Ecommerce_AppContext _db;
        private readonly UserManager<Ecommerce_AppUser> _userManager;
        private readonly SignInManager<Ecommerce_AppUser> _signInManager;
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;
        private readonly IEmailService _emailService;
        public readonly ILoggerService _logger;

        public HomeController(Infrastructure.Data.Ecommerce_AppContext db,
            UserManager<Ecommerce_AppUser> userManager,
            SignInManager<Ecommerce_AppUser> signInManager,
            IReservationService reservationService,
            IRoomService roomService,
            IEmailService emailService,
            ILoggerService logger)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _reservationService = reservationService;
            _roomService = roomService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var allRooms = await _roomService.GetAllRoom();
                var roomTypeItems = allRooms.Select(rt => new SelectListItem
                {
                    Value = rt.RoomId.ToString(),
                    Text = rt.RoomType.NameEn.ToString()
                }).ToList();

                var model = new LookUpPropertyIndexView
                {
                    Rooms = allRooms.ToList(),
                    RoomTypes = roomTypeItems
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting Home", ex);
                return View("page-500");
            }
        }     

        public async Task<IActionResult> Reservation(int roomId)
        {
            try
            {
                var room = await _roomService.GetId(roomId);

                if (room.Status == true || room.RoomId == 0)
                {
                    return View("page-404");
                }

                var model = new Reservation
                {
                    RoomId = roomId
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting Reservation", ex);
                return View("page-500");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservation(Reservation reservation)
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

                    var userId = _userManager.GetUserId(User);

                    if (string.IsNullOrEmpty(userId))
                    {
                        var guestUser = await _db.AspNetUsers.SingleOrDefaultAsync(u => u.Email == "Guest@gmail.com");
                        userId = guestUser.Id;
                    }

                    await _reservationService.Add(reservation, userId);

                    Response.Cookies.Append("ReservationMade", "true");

                    return RedirectToAction("ThankYou");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while making a reservation", ex);
                return View();
            }
        }

        public IActionResult ThankYou()
        {
            try
            {
                if (Request.Cookies["ReservationMade"] == "true")
                {
                    Response.Cookies.Append("ReservationMade", "false");
                    return View();
                }
                else
                {
                    return View("page-404");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occurred while getting Thank You", ex);
                return View("page-500");

            }

        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                string recipientEmail = "info@pretalhotel.com";
                string body = $"Name: {contact.Name}<br>Email: {contact.Email}<br>Message: {contact.Message}";

                try
                {
                    _emailService.SendEmail(contact.Email, recipientEmail, "Pretal Hotel", body);
                    return Json(new { success = true, message = "Message sent successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred. Please try again." });
                }
            }

            return Json(new { success = false, message = "Validation errors occurred." });
        }

        public async Task<IActionResult> Rooms()
        {
            try
            {
                var rooms = await _roomService.GetAllActiveRooms();
                return View(rooms);
            }
            catch(Exception ex)
            {
                _logger.LogError("There is an error while getting rooms",ex);
                return View("page-500");
            }
        }

        public async Task<IActionResult> RoomDetails(int id)
        {
            try
            {
                var room = await _roomService.GetRoomByRoomId(id);

                if (room.Status == true || room.RoomId == 0)
                {
                    return View("page-404");
                }

                return View(room);
            }
            catch (Exception ex)
            {
                _logger.LogError("There is an error while getting room details", ex);
                return View("page-500");
            }

        }

        public IActionResult AboutUS()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRoomCapacity(int roomId)
        {
            try
            {
                var roomCapacity = await _roomService.GetRoomCapacity(roomId);
                return Json(roomCapacity);
            }
            catch(Exception ex)
            {
                _logger.LogError("There is an error while getting room capacity", ex);
                return View("page-500");
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
                    return View("page-404");
                }

                var roomTypeId = room.RoomTypeId;
                var roomType = await _db.RoomTypes.SingleOrDefaultAsync(x => x.TypeId == roomTypeId);

                if (roomType == null)
                {
                    return View("page-404");
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
                return View("page-500");
            }
        }

    }
}


