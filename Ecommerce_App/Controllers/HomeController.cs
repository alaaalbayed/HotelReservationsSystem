using Domain.DTO_s;
using Domain.Interface;
using Domain.Models;
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
        private readonly IReservationService _reservationService;
        public HomeController(IAnalyticService analyticService, IReservationService reservationService, ILoggerService logger) : base(logger)
        {
            _analyticService = analyticService;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var totalReservations = await _analyticService.TotalReservationsNumber();
                var totalRevenu = await _analyticService.TotalRevenue();
                var totalIncome = await _analyticService.TotalIncome();
                var getAllReservations = await _reservationService.GetAllReservations();

                var viewModel = new AnalyticsViewModel
                {
                    TotalReservations = totalReservations,
                    TotalRevenue = totalRevenu,
                    TotalIncome = totalIncome,
                    Reservations = getAllReservations
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred in the HomeController Index action.", ex);
                return NotFound500();
            }
        }
    }
}
