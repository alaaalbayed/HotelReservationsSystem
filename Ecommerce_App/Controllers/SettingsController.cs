using Domain.Interface;
using Ecommerce_App.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class SettingsController : BaseController
    {
        private readonly UserManager<Ecommerce_AppUser> _userManager;
        private readonly SignInManager<Ecommerce_AppUser> _signInManager;

        public SettingsController(
            UserManager<Ecommerce_AppUser> userManager,
            SignInManager<Ecommerce_AppUser> signInManager,
            ILoggerService logger
            ) : base(logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _signInManager.UserManager.GetUserAsync(User);
                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting users", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is an error while trying to get the edit page!", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }



        [HttpPost]
        public async Task<IActionResult> Edit(Ecommerce_AppUser user, string newPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await UploadImage(user);
                    var existingUser = await _userManager.GetUserAsync(User);

                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Address = user.Address;
                    existingUser.Image = user.Image;

                    if (!string.IsNullOrWhiteSpace(newPassword))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                        await _userManager.ResetPasswordAsync(existingUser, token, newPassword);
                    }

                    var result = await _userManager.UpdateAsync(existingUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is an error while trying to edit!", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

    }
}
