using Domain.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class AnalyticsController : BaseController
    {
        private readonly IAnalyticService _analyticService;
        private readonly IReservationService _reservationService;
        private readonly IVisitorService _visitorService;
        public AnalyticsController(
            IAnalyticService analyticService,
            IReservationService reservationService,
            IVisitorService visitorService,
            ILoggerService logger) : base(logger)
        {
            _analyticService = analyticService;
            _reservationService = reservationService;
            _visitorService = visitorService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {

                int pageSize = 7;

                var totalReservations = await _analyticService.TotalReservationsNumber();
                var totalIncome = await _analyticService.TotalIncome();
                var getAllReservations = await _reservationService.GetAllReservations();
                var getTotalUsers = await _analyticService.TotalUsers();
                var getTotalAdmins = await _analyticService.TotalAdmins();
                var getTotalEmployees = await _analyticService.TotalEmployees();
                var getTotalVisitors = await _visitorService.GetVisitorsCount();

                var totalPages = (int)Math.Ceiling(totalReservations / (double)pageSize);
                var viewModel = new AnalyticsViewModel
                {
                    TotalReservations = totalReservations,
                    Reservations = getAllReservations.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalIncome = totalIncome,
                    TotalUsers = getTotalUsers,
                    TotalAdmins = getTotalAdmins,
                    TotalEmployees = getTotalEmployees,
                    TotalVisitors = getTotalVisitors

                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred in the HomeController Index action.", ex);
                return NotFound500();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMonthlySale(string year, int month)
        {
            var monthlySale = await _analyticService.GetMonthlySale(year, month);
            return Json(monthlySale);
        }

        [HttpGet]
        public async Task<IActionResult> GetMonthlyOrder(string year, int month)
        {
            var monthlyOrder = await _analyticService.GetMonthlyOrder(year, month);
            return Json(monthlyOrder);
        }
    }
}
