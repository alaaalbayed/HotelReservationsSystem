using Domain.DTO_s;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class LookUpTypesController : BaseController
    {
        private readonly ILookUpTypeService _lookUpTypeService;


        public LookUpTypesController(ILookUpTypeService lookUpTypeService, ILoggerService logger) : base (logger)
        {
            _lookUpTypeService = lookUpTypeService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var allLookUpTypes = await _lookUpTypeService.GetAllLookUpTypes();
                return View(allLookUpTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room types", ex);
                return NotFound500();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LookUpType lookUpType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _lookUpTypeService.Add(lookUpType);
                    return RedirectToAction(nameof(Index));
                }
                return View(lookUpType);
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

                var lookUpType = await _lookUpTypeService.GetLookUpTypeById(id);

                if (lookUpType == null)
                {
                    return NotFound404();
                }

                return View(lookUpType);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting room type for editing", ex);
                return NotFound500();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LookUpType lookUpType)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(lookUpType);
                }

                await _lookUpTypeService.Update(id, lookUpType);
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

                var roomType = await _lookUpTypeService.GetLookUpTypeById(id);

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
                await _lookUpTypeService.Delete(id);
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
