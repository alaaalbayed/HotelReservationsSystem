using Domain.DTO_s;
using Domain.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class LookUpPropertyController : BaseController
    {
        private readonly ILookUpPropertyService _lookUpPropertyService;
        private readonly ILookUpTypeService _lookUpTypeService;

        public LookUpPropertyController(ILookUpPropertyService lookUpPropertyService, ILookUpTypeService lookUpTypeService, ILoggerService logger) : base(logger)
        {
            _lookUpPropertyService = lookUpPropertyService;
            _lookUpTypeService = lookUpTypeService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var allLookUpProperty = await _lookUpPropertyService.GetAllLookUpProperty();
                var check = CultureInfo.CurrentCulture.Name;
                foreach (var property in allLookUpProperty)
                {
                    property.LookUpType = new LookUpType();
                    if(check == "en-US") { 
                        property.LookUpType.NameEn = await _lookUpPropertyService.GetTypeNameByRoomTypeId(property.TypeId);
                    }
                    else
                    {
                        property.LookUpType.NameAr = await _lookUpPropertyService.GetTypeNameByRoomTypeId(property.TypeId);
                    }
                }

                return View(allLookUpProperty);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting LookUpProperty", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var lookUpTypes = await _lookUpTypeService.GetAllLookUpTypes();
                var typeOptions = lookUpTypes.Select(lt => new SelectListItem { Value = lt.Id.ToString(), Text = lt.NameEn }).ToList();
                ViewBag.TypeOptions = typeOptions;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while view create LookUpProperty page", ex);
                return NotFound500();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LookUpProperty lookUpProperty)
        {
            try
            {
                await _lookUpPropertyService.Add(lookUpProperty);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a LookUpProperty", ex);
                return NotFound500();
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            try
            {

                var lookUpProperty = await _lookUpPropertyService.GetLookUpPropertyById(id);

                if (lookUpProperty == null)
                {
                    return NotFound404();
                }

                var allLookUpTypes = await _lookUpTypeService.GetAllLookUpTypes();

                var viewModel = new EditLookUpPropertyViewModel
                {
                    LookUpProperty = lookUpProperty,
                    AllLookUpTypes = allLookUpTypes
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting LookUpProperty for editing", ex);
                return NotFound500();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditLookUpPropertyViewModel viewModel)
        {
            try
            {
                await _lookUpPropertyService.Update(id, viewModel.LookUpProperty);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating LookUpProperty", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Info(int id)
        {
            try
            {

                var roomType = await _lookUpPropertyService.GetByRoomTypeId(id);
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
                await _lookUpPropertyService.Delete(id);
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
