using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BaseController : Controller
    {
        private readonly IStringLocalizer<BaseController> _localizer;
        private readonly ILogger<BaseController> _logger;

        public BaseController(IStringLocalizer<BaseController> localizer, ILogger<BaseController> logger)
        {
            _localizer = localizer;
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
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
