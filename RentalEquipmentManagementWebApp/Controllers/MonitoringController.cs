using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementSystem_WebApplication.Models.ViewModels;
using RentalEquipmentManagementLogic.Models; // Replace with your actual namespace for DbContext


namespace RentalEquipmentManagementWebApp.Controllers
{
    public class MonitoringController : Controller
    {
        private readonly EquipmentRentalDBContext _context;

        public MonitoringController(EquipmentRentalDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Dashboard()
        {
            var viewModel = new DashboardViewModel();

            // Total Active Rentals
            viewModel.TotalActiveRentals = await _context.RentalTransactions
          .CountAsync(rt => rt.ReturnDate >= DateTime.Now && !_context.ReturnRecords.Any(rr => rr.RentalTransactionId == rt.Id));

            // Total Available Equipment
            viewModel.TotalAvailableEquipment = await _context.Equipment
                .CountAsync(e => e.AvailabilityStatus == "Available");

            // Total Equipment
            viewModel.TotalEquipment = await _context.Equipment.CountAsync();

            // New Pending Rental Requests (last 7 days)
            viewModel.NewPendingRequests = await _context.RentalRequests
                .CountAsync(rr => rr.Status == "Pending" && rr.CreatedAt >= DateTime.Now.AddDays(-7));

            // Equipment Availability by Status
            viewModel.EquipmentAvailability = await _context.Equipment
                .GroupBy(e => e.AvailabilityStatus)
                .Select(g => new EquipmentAvailabilityStatus { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            // Total Revenue Last Month
            viewModel.TotalRevenueLastMonth = await _context.RentalTransactions
                .Where(rt => rt.CreatedAt.Value.Year == DateTime.Now.Year && rt.CreatedAt.Value.Month == DateTime.Now.Month)
                .SumAsync(rt => rt.RentalFee);

            // Pending Rental Requests by Category
            viewModel.PendingRequestsByCategory = await _context.RentalRequests
            .Where(rr => rr.Status == "Pending")
                .Join(_context.Equipment, rr => rr.EquipmentId, e => e.Id, (rr, e) => new { rr, e })
                .Join(_context.Categories, eq => eq.e.CategoryId, c => c.Id, (eq, c) => new { eq, c })
                .GroupBy(g => g.c.Name)
                .Select(g => new CategoryPendingRequests { CategoryName = g.Key, PendingRequestsCount = g.Count() })
                .ToListAsync();

            // Damaged Equipment Report
            viewModel.DamagedEquipmentReport = await _context.Equipment
                .Where(e => e.ConditionStatus == "Damaged")
                .Select(e => new DamagedEquipment { EquipmentName = e.Name, ConditionStatus = e.ConditionStatus })
                .ToListAsync();

            // Overdue Rentals
            viewModel.OverdueRentals = await _context.RentalTransactions
                .Where(rt => rt.ReturnDate < DateTime.Now && !_context.ReturnRecords.Any(rr => rr.RentalTransactionId == rt.Id))
                .Join(_context.Equipment, rt => rt.AssignedEquipmentId, e => e.Id, (rt, e) => new { rt, e })
                .Select(re => new OverdueRental { TransactionId = re.rt.Id, EquipmentName = re.e.Name, ReturnDate = re.rt.ReturnDate })
                .ToListAsync();

            // Average Feedback Rating
            viewModel.AverageFeedbackRating = await _context.Feedbacks
                .AverageAsync(f => (double?)f.Rating) ?? 0;

            return View(viewModel);
        }

        private string GetCurrentUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            }
            return null;
        }
    }
}

