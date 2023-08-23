using Domain.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                var getTotalUsers = await _analyticService.TotalUsers();
                var getTotalAdmins = await _analyticService.TotalAdmins();
                var getTotalEmployees = await _analyticService.TotalEmployees();

                var viewModel = new AnalyticsViewModel
                {
                    TotalReservations = totalReservations,
                    TotalRevenue = totalRevenu,
                    TotalIncome = totalIncome,
                    Reservations = getAllReservations,
                    TotalUsers = getTotalUsers,
                    TotalAdmins = getTotalAdmins,
                    TotalEmployees = getTotalEmployees,
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
