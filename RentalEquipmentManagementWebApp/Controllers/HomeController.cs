using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RentalEquipmentManagementWebApp.Models;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Action for the Home page
        public IActionResult Index()
        {
            // Set the active page for navigation
            ViewData["ActivePage"] = "Home";

            return View();
        }

        // Action for the Privacy page
        public IActionResult Privacy()
        {
            // Set the active page for navigation
            ViewData["ActivePage"] = "Privacy";

            return View();
        }

        // Error page handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
