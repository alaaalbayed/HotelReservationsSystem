/*using Ecommerce_App.Models;*/
using Domain.DTO_s;
using Domain.Interface;
using Ecommerce_App.Areas.Identity.Data;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace Ecommerce_App.Controllers
{
    public class HomeController : BaseController
	{
		private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> Localizer) :base(Localizer)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}