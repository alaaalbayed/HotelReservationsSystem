using Domain.DTO_s;
using Domain.Interface;
using Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using orm = Infrastructure.Data;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomsController : BaseController
    {
        private readonly orm.Ecommerce_AppContext _db;
        private readonly IRoomService _roomService;
        private readonly ILookUpTypeService _lookUpTypeService;
        private readonly IRoomImageService _roomImageService;
        private readonly ILookUpPropertyService _lookUpPropertyService;


        public RoomsController(
            orm.Ecommerce_AppContext db,
            IRoomService roomService,
            ILookUpTypeService lookUpTypeService,
            IRoomImageService roomImageService,
            ILookUpPropertyService lookUpPropertyService,
            ILoggerService logger) : base(logger)
        {
            _db = db;
            _roomService = roomService;
            _lookUpTypeService = lookUpTypeService;
            _roomImageService = roomImageService;
            _lookUpPropertyService = lookUpPropertyService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var rooms = await _roomService.GetAllRoom();
                return View(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room data", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var availableRoomTypes = await _lookUpPropertyService.GetAllLookUpProperty();
                var typeOptions = availableRoomTypes.Select(lt => new SelectListItem { Value = lt.Id.ToString(), Text = lt.NameEn }).ToList();
                ViewBag.TypeOptions = typeOptions;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while preparing room creation", ex);
                return NotFound500();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room model)
        {
            try
            {
                var selectedRoomType = await _lookUpPropertyService.GetByRoomTypeId(model.RoomTypeId);

                if (selectedRoomType != null)
                {
                    model.NameEn = selectedRoomType.NameEn;
                    model.NameAr = selectedRoomType.NameAr;
                    model.RoomTypeId = selectedRoomType.Id;

                    await _roomService.Add(model, model.RoomImages);
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a room", ex);
                return NotFound500();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var room = await _db.Rooms.Include(r => r.RoomImages).FirstOrDefaultAsync(r => r.RoomId == id);

                if (room == null)
                {
                    return NotFound404();
                }

                var viewModel = new Room
                {
                    Capacity = room.Capacity,
                    PricePerNight = room.PricePerNight,
                    RoomNumber = room.RoomNumber,
                    RoomTypeId = room.RoomTypeId,
                };

                var availableRoomTypes = await _lookUpPropertyService.GetAllLookUpProperty();
                viewModel.RoomTypes = availableRoomTypes.Select(rt => new SelectListItem
                {
                    Value = rt.Id.ToString(),
                    Text = rt.NameEn.ToString()
                }).ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while preparing room editing", ex);
                return NotFound500();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room viewModel, List<int> imageIds)
        {
            try
            {
                var existingRoom = await _roomService.GetId(id);

                if (existingRoom == null)
                {
                    return NotFound404();
                }

                if (!await _roomService.IsRoomNumberFree(viewModel.RoomNumber, id))
                {
                    ModelState.AddModelError(nameof(viewModel.RoomNumber), "Number with the same Id already exists");
                }

                existingRoom.Capacity = viewModel.Capacity;
                existingRoom.RoomNumber = viewModel.RoomNumber;
                existingRoom.PricePerNight = viewModel.PricePerNight;
                existingRoom.RoomTypeId = viewModel.RoomTypeId;

                if (!viewModel.KeepExistingImages)
                {
                    if (existingRoom.RoomImages2 != null && existingRoom.RoomImages2.Any())
                    {
                        await _roomImageService.RemoveAll(existingRoom.RoomId);
                    }
                }

                await _roomService.Update(id, existingRoom, viewModel.RoomImages);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating a room", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _roomService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting a reservation", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Info(int id)
        {
            try
            {
                var room = await _roomService.GetId(id);

                if (room == null)
                {
                    return NotFound404();
                }

                return View(room);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room info", ex);
                return NotFound500();
            }
        }
    }
}
