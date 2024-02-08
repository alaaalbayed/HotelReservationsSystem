using Domain.Interface;
using Ecommerce_App.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : BaseController
    {
        private readonly UserManager<Ecommerce_AppUser> _userManager;

        public UsersController(UserManager<Ecommerce_AppUser> userManager, ILoggerService logger) : base(logger)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var allUsers = await _userManager.Users.ToListAsync();
                return View(allUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting users", ex);
                return NotFound500();
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ecommerce_AppUser user, string newPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await UploadImage(user);
                    user.UserName = user.Email;

                    var existingUser = await _userManager.FindByEmailAsync(user.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email already exists. Please use a different email address.");
                        return View(user);
                    }

                    var result = await _userManager.CreateAsync(user, newPassword);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is an error while trying to create a user.", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var user = users.FirstOrDefault(u => u.Id == id.ToString());

                if (user == null)
                {
                    return NotFound404();
                }

                return View(user);
            }

            catch (Exception ex)
            {
                _logger.LogError($"There is error while trying to get the edit page!", ex);
                return NotFound500();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Ecommerce_AppUser user, string newPassword)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(user.Id);

                if (ModelState.IsValid)
                {
                    await UploadImage(user);
                   
                    if (existingUser == null)
                    {
                        return NotFound404();
                    }

                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Email = user.Email;
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
                _logger.LogError($"There is error while trying edit!", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Info(Guid? id)
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var user = users.FirstOrDefault(u => u.Id == id.ToString());

                if (user == null)
                {
                    return NotFound404();
                }

                return View(user);
            }

            catch (Exception ex)
            {
                _logger.LogError("There is error while trying to get the info page!", ex);
                return NotFound500();
            }
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());

                if (user == null)
                {
                    return NotFound500();
                }

                user.Status = !user.Status;
                var result = await _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("There is error while trying to delete", ex);
                return NotFound500();
            }
        }
    }
}
