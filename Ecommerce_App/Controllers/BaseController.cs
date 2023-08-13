using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Localization;

namespace Ecommerce_App.Controllers
{
	[Authorize(Roles ="Admin")]
    public class BaseController : Controller
	{
		public readonly IStringLocalizer<BaseController> _Localizer;
        public BaseController()
        {

        }
        public BaseController(IStringLocalizer<BaseController> Localizer)
		{
			_Localizer = Localizer;

        }

		[HttpPost]
		public IActionResult SetLanguage(string culture, string returnUrl)
		{
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
			return LocalRedirect(returnUrl);
		}

    }
}
