using Domain.DTO_s;
using Domain.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomTypesController : BaseController
    {
        private readonly IRoomTypeService _roomTypeService;
        private readonly ISettingService _setting;

        public RoomTypesController(IRoomTypeService roomTypeService, ISettingService setting)
        {
            _roomTypeService = roomTypeService;
            _setting = setting;
        }

        public IActionResult Index()
        {
            var allRoomTypes = _roomTypeService.GetAllRoomTypes();
            ViewBag.RoomTypes = allRoomTypes;
            return View(allRoomTypes);
        }

        public IActionResult Create()
        {
            var allRoomTypes = _setting.GetAllRoomTypes();
            var model = new LookUpRoomType
            {
                RoomTypes = allRoomTypes.Select(rt => new SelectListItem
                {
                    Value = rt.Id.ToString(),
                    Text = rt.NameEn
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LookUpProperty roomTypeIn)
        {

            if (ModelState.IsValid)
            {
                await _roomTypeService.Add(roomTypeIn);
                return RedirectToAction(nameof(Index));
            }
            return View(roomTypeIn);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var roomType = await _roomTypeService.GetRoomTypeById(id);
            if (roomType == null)
            {
                return NotFound();
            }

            var allRoomTypes = _setting.GetAllRoomTypes();
            var model = new LookUpProperty
            {
                RoomTypes = allRoomTypes.Select(rt => new SelectListItem
                {
                    Value = rt.Id.ToString(),
                    Text = rt.NameEn

                }).ToList()
            };

            var viewModel = new EditRoomTypeViewModel
            {
                LookUpRoomType = new LookUpRoomType
                {
                    Id = roomType.Id,
                    NameAr = roomType.NameAr,
                    NameEn = roomType.NameEn
                },
                LookUpProperty = model
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LookUpProperty roomType)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(roomType);
            }
            await _roomTypeService.Update(id, roomType);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Info(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _roomTypeService.GetRoomTypeById(id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            await _roomTypeService.Delete(id);
            return Ok();
        }
    }
}
