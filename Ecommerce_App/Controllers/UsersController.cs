using Domain.DTO_s;
using Domain.Interface;
using Domain.Service;
using Ecommerce_App.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.VisualBasic;

namespace Ecommerce_App.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly UserManager<Ecommerce_AppUser> _userManager;
        private readonly IStringLocalizer _localizer;
        private readonly ILoggerService _logger;

        public UsersController(UserManager<Ecommerce_AppUser> userManager, IStringLocalizer<HomeController> Localizer, ILoggerService logger) : base(Localizer)
        {
            _userManager = userManager;
            _localizer = Localizer;
            _logger = logger;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ecommerce_AppUser user)
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
                int result = Math(1, 0);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while doing something.", ex);
            }

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

        private async Task UploadImage(Ecommerce_AppUser user)
        {
            try
            {
                var file = HttpContext.Request.Form.Files;
                if (file.Count > 0)
                {
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                    var fileStream = new FileStream(Path.Combine(@"wwwroot/", "Images", ImageName), FileMode.Create);
                    await file[0].CopyToAsync(fileStream);
                    user.Image = ImageName;
                }
                else if (user.Image == null && user.Id == null)
                {
                    user.Image = "DefaultImage.jpg";
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"There is error while trying to upload the image", ex);
            }
            
        }

        public int Math(int x, int y)
        {
            return x / y;
        }
    }
}
