﻿using Domain.Interface;
using Domain.Service;
using Ecommerce_App.Areas.Identity.Data;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminsController : BaseController
    {
        private readonly UserManager<Ecommerce_AppUser> _userManager;
        private readonly Ecommerce_AppContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminsController(UserManager<Ecommerce_AppUser> userManager, Ecommerce_AppContext db, RoleManager<IdentityRole> roleManager, ILoggerService logger) : base(logger)
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
                _logger.LogError("An error occurred while getting users", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
        public async Task<IActionResult> Create()
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
                _logger.LogError($"There is an error while trying to create a user.", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var users = await _userManager.Users.ToListAsync();
                var user = users.FirstOrDefault(u => u.Id == id.ToString());

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }

            catch (Exception ex)
            {
                _logger.LogError($"There is error while trying to get the edit page!", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Ecommerce_AppUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await UploadImage(user);
                    var existingUser = await _userManager.FindByIdAsync(user.Id);

                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Address = user.Address;
                    existingUser.Image = user.Image;

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
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> InfoAsync(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var users = await _userManager.Users.ToListAsync();
                var user = users.FirstOrDefault(u => u.Id == id.ToString());

                if (user == null)
                {
                    return NotFound();
                }

                return View("Info", user);
            }

            catch (Exception ex)
            {
                _logger.LogError("There is error while trying to get the info page!", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
                if (user == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                user.Status = !user.Status;
                var result = await _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("There is error while trying to delete", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }

        }
    }
}
