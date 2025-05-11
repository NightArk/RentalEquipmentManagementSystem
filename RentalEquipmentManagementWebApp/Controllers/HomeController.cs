using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models;
using RentalEquipmentManagementWebApp.Models.Home;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EquipmentRentalDBContext _context;

        public HomeController(ILogger<HomeController> logger, EquipmentRentalDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                CategoryCount = await _context.Categories.CountAsync(),
                EquipmentCount = await _context.Equipment.CountAsync(),
                AvailableEquipmentCount = await _context.Equipment.CountAsync(e => e.AvailabilityStatus == "Available"),
                FeaturedEquipment = await _context.Equipment
                    .Include(e => e.Category)
                    .Where(e => e.AvailabilityStatus == "Available")
                    .OrderByDescending(e => e.CreatedAt)
                    .Take(6)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
