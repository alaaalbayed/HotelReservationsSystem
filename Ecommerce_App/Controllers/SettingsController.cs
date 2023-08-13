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
        private readonly ILoggerService _logger;

        public SettingsController(ISettingService setting, ILoggerService logger)
        {
            _setting = setting;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var allRoomTypes = _setting.GetAllRoomTypes();
                return View(allRoomTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room types", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LookUpRoomType roomTypeIn)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _setting.Add(roomTypeIn);
                    return RedirectToAction(nameof(Index));
                }
                return View(roomTypeIn);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a room type", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room type for editing", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LookUpRoomType roomType)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating room type", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> Info(int id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room type info", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                await _setting.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting room type", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
