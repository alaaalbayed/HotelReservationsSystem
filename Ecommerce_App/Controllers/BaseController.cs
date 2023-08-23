using Domain.Interface;
using Ecommerce_App.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class BaseController : Controller
    {
        public readonly ILoggerService _logger;

        public BaseController(ILoggerService logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            try
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while setting language.", ex);
                return NotFound500();
            }
        }

        public async Task UploadImage(Ecommerce_AppUser user)
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
            catch (Exception ex)
            {
                _logger.LogError($"There is error while trying to upload the image", ex);
            }
        }
        public IActionResult NotFound404()
        {
            return View("page-404");
        }

        public IActionResult NotFound500()
        {
            return View("page-500");
        }
    }
}
