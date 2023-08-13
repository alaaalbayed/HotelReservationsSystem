using Domain.DTO_s;
using Domain.Interface;
using Ecommerce_App.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomsController : BaseController
    {
        private readonly Infrastructure.Data.Ecommerce_AppContext _db;
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IRoomImageService _roomImageService;

        public RoomsController(Infrastructure.Data.Ecommerce_AppContext db, IRoomService roomService, IRoomTypeService roomTypeService, IRoomImageService roomImageService)
        {
            _db = db;
            _roomService = roomService;
            _roomTypeService = roomTypeService;
            _roomImageService = roomImageService;
        }
        public async Task<IActionResult> Index()
        {
            var rooms = await _roomService.GetAllRoom();
            var roomTypes = _roomTypeService.GetAllRoomTypes();

            ViewBag.RoomTypes = roomTypes;
            return View(rooms);
        }

        public async Task<IActionResult> Create()
        {
            var availableRoomTypes = _roomTypeService.GetAllRoomTypes();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room model)
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
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

            var availableRoomTypes = _roomTypeService.GetAllRoomTypes();
            viewModel.RoomTypes = availableRoomTypes.Select(rt => new SelectListItem
            {
                Value = rt.TypeId.ToString(),
                Text = rt.NameEn
            }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room viewModel, List<int> imageIds)
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

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            await _roomService.Delete(id);

            return Ok();
        }

        public async Task<IActionResult> Info(int id)
        {
            var room = await _roomService.GetId(id);

            var roomTypes = _roomTypeService.GetAllRoomTypes();

            ViewBag.RoomTypes = roomTypes;

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }
    }
}
