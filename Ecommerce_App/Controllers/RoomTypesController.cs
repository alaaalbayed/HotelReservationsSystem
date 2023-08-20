using Domain.DTO_s;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_App.Controllers
{
    public class RoomTypesController : BaseController
    {
        private readonly IRoomTypeService _roomTypeService;
        private readonly ILookUpTypeService _lookUpTypeService;
        private readonly ILookUpPropertyService _lookUpPropertyService;

        public RoomTypesController(IRoomTypeService roomTypeService, ILookUpTypeService lookUpTypeService, ILookUpPropertyService lookUpPropertyService, ILoggerService logger) : base(logger)
        {
            _roomTypeService = roomTypeService;
            _lookUpTypeService = lookUpTypeService;
            _lookUpPropertyService = lookUpPropertyService;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var allRoomTypes = await _roomTypeService.GetAllRoomType();
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
            var lookUpProperty = await _lookUpPropertyService.GetAllLookUpProperty();
            var typeOptions = lookUpProperty.Select(lt => new SelectListItem { Value = lt.Id.ToString(), Text = lt.NameEn }).ToList();
            ViewBag.TypeOptions = typeOptions;

            return View();
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
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
