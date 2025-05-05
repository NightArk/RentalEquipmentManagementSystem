using Microsoft.AspNetCore.Mvc;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class RentalTransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
