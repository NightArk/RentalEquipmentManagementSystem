using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.ViewModels;
using System.Security.Claims;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly EquipmentRentalDBContext _context;
    


        public FeedbackController(EquipmentRentalDBContext context)
        {
            _context = context;
        }

        // GET: Feedback/Equipment/{equipmentId}
        // GET: Feedback/Equipment/{equipmentId}
        public async Task<IActionResult> Index(int? equipmentId)
        {
            if (equipmentId == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment.FindAsync(equipmentId);
            if (equipment == null)
            {
                return NotFound();
            }

            ViewBag.EquipmentName = equipment.Name;
            ViewBag.EquipmentId = equipmentId;

            var feedbackQuery = _context.Feedbacks
                .Include(f => f.User)
                .Where(f => f.EquipmentId == equipmentId);

            if (!User.IsInRole("Manager") && !User.IsInRole("Admin"))
            {
                // If the user is a customer, show only non-hidden feedback
                feedbackQuery = feedbackQuery.Where(f => !f.IsHidden);
            }

            var feedbackViewModels = await feedbackQuery.Select(f => new FeedbackViewModel
            {
                Id = f.Id,
                CustomerName = f.User != null ? f.User.Name : string.Empty,
                Comment = f.Comment,
                Rating = f.Rating,
                CreatedAt = (DateTime)f.CreatedAt,
                IsHidden = f.IsHidden // Pass IsHidden status to the view
            })
            .OrderByDescending(vm => vm.CreatedAt)
            .ToListAsync();

            return View(feedbackViewModels);
        }
        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out var userId) ? (int?)userId : null;
        }

        private bool IsCurrentUserCustomer()
        {
            return User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Customer");
        }

        // GET: Feedback/Create/{equipmentId}
        public async Task<IActionResult> Create(int? equipmentId)
        {
            if (equipmentId == null)
            {
                return NotFound();
            }

            //var hasRentedBefore = await _context.RentalTransactions
            //    .AnyAsync(rt => rt.CustomerId == GetCurrentUserId().Value &&
            //         rt.AssignedEquipmentId == equipmentId &&
            //         rt.ReturnDate != null);

            //if (!hasRentedBefore)
            //{
            //    ViewBag.ErrorMessage = "You haven't rented this equipment before or the rental is not yet returned.";
            //    ViewBag.EquipmentId = equipmentId;
            //    return View("NoFeedbackAllowed");
            //}

            var equipment = await _context.Equipment.FindAsync(equipmentId);
            if (equipment == null)
            {
                return NotFound();
            }

            var viewModel = new CreateFeedbackViewModel
            {
                EquipmentId = equipmentId.Value,
                EquipmentName = equipment.Name
            };

            return View(viewModel);
        }

        // POST: Feedback/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeedbackViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = GetCurrentUserId();
                if (!currentUserId.HasValue)
                {
                    return Unauthorized();
                }

                var feedback = new Feedback
                {
                    UserId = currentUserId.Value,
                    EquipmentId = viewModel.EquipmentId,
                    Rating = viewModel.Rating,
                    Comment = viewModel.Comment,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { equipmentId = viewModel.EquipmentId });
            }

            // Repopulate EquipmentName on error
            var equipment = await _context.Equipment.FindAsync(viewModel.EquipmentId);
            if (equipment != null)
            {
                viewModel.EquipmentName = equipment.Name;
            }

            return View(viewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Manage(int? feedbackId, int? equipmentId)
        {
            if (feedbackId == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.Equipment)
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == feedbackId);

            if (feedback == null)
            {
                return NotFound();
            }

            var viewModel = new FeedbackViewModel
            {
                Id = feedback.Id,
                CustomerName = feedback.User?.Name,
                Comment = feedback.Comment,
                Rating = feedback.Rating,
                CreatedAt = (DateTime)feedback.CreatedAt,
                IsHidden = feedback.IsHidden
            };

            ViewBag.EquipmentId = equipmentId; // To pass back for the "Back to List" link

            return View(viewModel);
        }

        // 3. Hide Feedback (POST) - For Managers and Admins to hide feedback
        [Authorize(Roles = "Manager, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Hide(int id, int? equipmentId)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.IsHidden = true; // Setting the C# boolean property
            await _context.SaveChangesAsync(); // EF Core will translate this to BIT 1

            return RedirectToAction(nameof(Index), new { feedbackId = id, equipmentId = equipmentId });
        }

        // 4. Show Feedback (POST) - For Managers and Admins to show feedback
        [Authorize(Roles = "Manager, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Show(int id, int? equipmentId)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.IsHidden = false; // Setting the C# boolean property
            await _context.SaveChangesAsync(); // EF Core will translate this to BIT 0

            return RedirectToAction(nameof(Index), new { feedbackId = id, equipmentId = equipmentId });
        }

        // 5. Delete Feedback (POST) - For Managers and Admins to delete feedback
        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int? equipmentId)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { equipmentId = equipmentId });
        }


    }
}
