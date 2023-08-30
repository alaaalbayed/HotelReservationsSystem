using Domain.DTO_s;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomTypesController : BaseController
    {
        private readonly IRoomTypeService _roomTypeService;
        private readonly ILookUpTypeService _lookUpTypeService;
        private readonly ILookUpPropertyService _lookUpPropertyService;

        public RoomTypesController(
            IRoomTypeService roomTypeService,
            ILookUpTypeService lookUpTypeService,
            ILookUpPropertyService lookUpPropertyService,
            ILoggerService logger) : base(logger)
        {
            _roomTypeService = roomTypeService;
            _lookUpTypeService = lookUpTypeService;
            _lookUpPropertyService = lookUpPropertyService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var allRoomTypes = await _roomTypeService.GetAllRoomType();
                return View(allRoomTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room types", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var lookUpProperty = await _lookUpPropertyService.GetAllLookUpProperty();
                var typeOptions = lookUpProperty.Select(lt => new SelectListItem { Value = lt.Id.ToString(), Text = lt.NameEn }).ToList();
                ViewBag.TypeOptions = typeOptions;

                return View();
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occurred while creation room type ", ex);
                return NotFound500();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomType roomType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _roomTypeService.Add(roomType);
                    return RedirectToAction(nameof(Index));
                }
                return View(roomType);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a room type", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {

                var lookUpProperty = await _lookUpPropertyService.GetAllLookUpProperty();
                var typeOptions = lookUpProperty.Select(lt => new SelectListItem { Value = lt.Id.ToString(), Text = lt.NameEn }).ToList();

                ViewBag.TypeOptions = typeOptions;

                var roomType = await _roomTypeService.GetById(id);

                if (roomType == null)
                {
                    return NotFound404();
                }

                return View(roomType);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room type for editing", ex);
                return NotFound500();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomType roomType)
        {
            try
            {
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
                return NotFound500();
            }
        }

        public async Task<IActionResult> Info(int id)
        {
            try
            {

                var roomType = await _roomTypeService.GetById(id);

                if (roomType == null)
                {
                    return NotFound404();
                }

                return View(roomType);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room type info", ex);
                return NotFound500();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                await _roomTypeService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting room type", ex);
                return NotFound500();
            }
        }

    }
}
