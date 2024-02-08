using Domain.Interface;
using Ecommerce_App.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminsController : BaseController
    {
        private readonly UserManager<Ecommerce_AppUser> _userManager;
        private readonly Ecommerce_AppContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminsController(
            UserManager<Ecommerce_AppUser> userManager,
            Ecommerce_AppContext db,
            RoleManager<IdentityRole> roleManager,
            ILoggerService logger) : base(logger)
        {
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
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
                _logger.LogError("An error occurred while getting Admins", ex);
                return NotFound500();
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ecommerce_AppUser user, string selectedRole, string newPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await UploadImage(user);
                    user.UserName = user.Email;

                    var result = await _userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrWhiteSpace(newPassword))
                        {
                            await _userManager.AddPasswordAsync(user, newPassword);
                        }

                        if (selectedRole == "Employee")
                        {
                            await _userManager.AddToRoleAsync(user, "Employee");
                        }
                        else if (selectedRole == "Admin")
                        {
                            await _userManager.AddToRoleAsync(user, "Admin");
                        }

                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is an error while trying to create a admin.", ex);
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
                _logger.LogError($"There is error while trying to get the edit admin page!", ex);
                return NotFound500();
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
                    var existingUser = await _userManager.FindByIdAsync(user.Id);

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
                _logger.LogError($"There is error while trying edit an admin.", ex);
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
                _logger.LogError("There is error while trying to get the admin info page", ex);
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
                    return NotFound404();
                }

                user.Status = !user.Status;
                var result = await _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("There is error while trying to delete an admin", ex);
                return NotFound500();
            }
        }
    }
}
