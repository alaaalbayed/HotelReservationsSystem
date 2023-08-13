using Domain.DTO_s;
using Domain.Interface;
using Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : BaseController
    {
        private readonly ISettingService _setting;

        public SettingsController(ISettingService setting)
        {
            _setting = setting;
        }

        public IActionResult Index()
        {
            var allRoomTypes = _setting.GetAllRoomTypes();
            return View(allRoomTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LookUpRoomType roomTypeIn)
        {
            if (ModelState.IsValid)
            {
                await _setting.Add(roomTypeIn);
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
            var roomType = await _setting.GetRoomTypeById(id);
            if (roomType == null)
            {
                return NotFound();
            }
            return View(roomType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LookUpRoomType roomType)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(roomType);
            }
            await _setting.Update(id, roomType);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Info(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _setting.GetRoomTypeById(id);
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

            await _setting.Delete(id);
            return Ok();
        }
    }
}
