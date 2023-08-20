using Domain.DTO_s;
using Domain.Interface;
using Domain.Models;
using Domain.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;

namespace Ecommerce_App.Controllers
{
    public class LookUpPropertyController : BaseController
    {
        private readonly ILookUpPropertyService _lookUpPropertyService;
        private readonly ILookUpTypeService _lookUpTypeService;


        public LookUpPropertyController(ILookUpPropertyService lookUpPropertyService, ILookUpTypeService lookUpTypeService, ILoggerService logger) : base(logger)
        {
            _lookUpPropertyService = lookUpPropertyService;
            _lookUpTypeService = lookUpTypeService;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var allLookUpProperty = await _lookUpPropertyService.GetAllLookUpProperty();
                return View(allLookUpProperty);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room types", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> Create()
        {
            var lookUpTypes = await _lookUpTypeService.GetAllLookUpTypes();
            var typeOptions = lookUpTypes.Select(lt => new SelectListItem { Value = lt.Id.ToString(), Text = lt.NameEn }).ToList();
            ViewBag.TypeOptions = typeOptions;

            return View();
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

                var lookUpProperty = await _lookUpPropertyService.GetLookUpPropertyById(id);
                if (lookUpProperty == null)
                {
                    return NotFound();
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
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditLookUpPropertyViewModel viewModel)
        {
            try
            {

                if (id <= 0)
                {
                    return NotFound();
                }

                await _lookUpPropertyService.Update(id, viewModel.LookUpProperty);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating LookUpProperty", ex);
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

                var roomType = await _lookUpTypeService.GetLookUpTypeById(id);
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

                await _lookUpTypeService.Delete(id);
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
