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

        public RoomTypesController(IRoomTypeService roomTypeService, ISettingService setting, ILoggerService logger) : base(logger)
        {
            _roomTypeService = roomTypeService;
            _setting = setting;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var allRoomTypes = await _roomTypeService.GetAllRoomTypes();
                ViewBag.RoomTypes = allRoomTypes;
                return View(allRoomTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room types", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var allRoomTypes = await _setting.GetAllRoomTypes();
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
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting data for creating room type", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LookUpProperty roomTypeIn)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _roomTypeService.Add(roomTypeIn);
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

                var roomType = await _roomTypeService.GetRoomTypeById(id);
                if (roomType == null)
                {
                    return NotFound();
                }

                var allRoomTypes = await _setting.GetAllRoomTypes();
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
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room type for editing", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LookUpProperty roomType)
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

                await _roomTypeService.Update(id, roomType);
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

                var roomType = await _roomTypeService.GetRoomTypeById(id);
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

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                await _roomTypeService.Delete(id);
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
