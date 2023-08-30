using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
