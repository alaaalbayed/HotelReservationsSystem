using Domain.DTO_s;
using Domain.Interface;
using Ecommerce_App.Areas.Identity.Data;
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
        private readonly IRoomTypeService _roomTypeService;
        private readonly IRoomImageService _roomImageService;


        public RoomsController(
            orm.Ecommerce_AppContext db,
            IRoomService roomService,
            IRoomTypeService roomTypeService,
            IRoomImageService roomImageService,
            ILoggerService logger
            ) : base(logger)
        {
            _db = db;
            _roomService = roomService;
            _roomTypeService = roomTypeService;
            _roomImageService = roomImageService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var rooms = await _roomService.GetAllRoom();
                var roomTypes = await _roomTypeService.GetAllRoomTypes();

                ViewBag.RoomTypes = roomTypes;
                return View(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room data", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var availableRoomTypes = await _roomTypeService.GetAllRoomTypes();

                var model = new Room
                {
                    RoomTypes = availableRoomTypes.Select(rt => new SelectListItem
                    {
                        Value = rt.TypeId.ToString(),
                        Text = rt.NameEn
                    }).ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while preparing room creation", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room model)
        {
            try
            {
                var selectedRoomType = await _roomTypeService.GetByTypeId(model.RoomTypeId);

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
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
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
                    return NotFound();
                }

                var viewModel = new Room
                {
                    Capacity = room.Capacity,
                    IsTaken = room.IsTaken,
                    AdultPrice = room.AdultPrice,
                    ChildrenPrice = room.ChildrenPrice,
                    RoomNumber = room.RoomNumber,
                    RoomTypeId = room.RoomTypeId,
                };

                var availableRoomTypes = await _roomTypeService.GetAllRoomTypes();
                viewModel.RoomTypes = availableRoomTypes.Select(rt => new SelectListItem
                {
                    Value = rt.TypeId.ToString(),
                    Text = rt.NameEn
                }).ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while preparing room editing", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
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
                    return NotFound();
                }

                if (!await _roomService.IsRoomNumberFree(viewModel.RoomNumber, id))
                {
                    ModelState.AddModelError(nameof(viewModel.RoomNumber), "Number with same Id already exists");
                }

                existingRoom.Capacity = viewModel.Capacity;
                existingRoom.RoomNumber = viewModel.RoomNumber;
                existingRoom.ChildrenPrice = viewModel.ChildrenPrice;
                existingRoom.IsTaken = viewModel.IsTaken;
                existingRoom.AdultPrice = viewModel.AdultPrice;
                existingRoom.RoomTypeId = viewModel.RoomTypeId;

                await _roomService.Update(id, existingRoom, viewModel.RoomImages);

                if (imageIds != null && imageIds.Any())
                {
                    foreach (var imageId in imageIds)
                    {
                        await _roomImageService.Remove(imageId);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating a room", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                await _roomService.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting a room", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> Info(int id)
        {
            try
            {
                var room = await _roomService.GetId(id);

                var roomTypes = await _roomTypeService.GetAllRoomTypes();

                ViewBag.RoomTypes = roomTypes;

                if (room == null)
                {
                    return NotFound();
                }

                return View(room);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room info", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
