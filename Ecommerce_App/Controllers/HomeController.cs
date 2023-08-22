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
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class HomeController : BaseController
    {
        private readonly IAnalyticService _analyticService;
        public HomeController(IAnalyticService analyticService, ILoggerService logger) : base(logger)
        {
            _analyticService = analyticService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var totalReservations = await _analyticService.TotalReservationsNumber();
                var totalRevenu = await _analyticService.TotalRevenue();
                var totalIncome = await _analyticService.TotalIncome();


                ViewBag.TotalReservations = totalReservations;
                ViewBag.TotalRevenu = totalRevenu;
                ViewBag.TotalIncome = totalIncome;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred in the HomeController Index action.", ex);
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
