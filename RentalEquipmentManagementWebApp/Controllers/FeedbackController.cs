using Microsoft.AspNetCore.Mvc;
using RentalEquipmentManagementWebApp.Models.Feedback;
using RentalEquipmentManagementWebApp.Data;
using RentalEquipmentManagementLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly EquipmentRentalDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;



        public FeedbackController(EquipmentRentalDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Feedback
        // Controller Action

        public async Task<IActionResult> Index(string searchUserName, int? filterEquipmentId, bool? filterIsHide)
        {
            var feedbackQuery = _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Equipment)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchUserName))
            {
                feedbackQuery = feedbackQuery.Where(f => f.User.Name.Contains(searchUserName));
            }

            if (filterEquipmentId.HasValue)
            {
                feedbackQuery = feedbackQuery.Where(f => f.EquipmentId == filterEquipmentId);
            }

            if (filterIsHide.HasValue)
            {
                feedbackQuery = feedbackQuery.Where(f => f.IsHide == filterIsHide);
            }

            var feedbacks = await feedbackQuery
                .Select(f => new FeedbackViewModel
                {
                    Id = f.Id,
                    UserId = (int)f.UserId,
                    EquipmentId = (int)f.EquipmentId,
                    Rating = (int)f.Rating,
                    Comment = f.Comment,
                    CreatedAt = (DateTime)f.CreatedAt,
                    IsHide = f.IsHide,
                    UserName = f.User.Name,
                    EquipmentName = f.Equipment.Name
                })
                .ToListAsync();

            // Filter dropdown
            var equipmentList = await _context.Equipment
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                })
                .ToListAsync();

            equipmentList.Insert(0, new SelectListItem { Value = "", Text = "All Equipment" });

            ViewBag.EquipmentList = equipmentList;
            ViewBag.SelectedEquipmentId = filterEquipmentId?.ToString() ?? "";
            ViewBag.UserSearch = searchUserName ?? "";

            return View(feedbacks);
        }






        [HttpPost]
        public async Task<IActionResult> Hide(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                TempData["ErrorMessage"] = "Feedback not found.";
                return RedirectToAction("Index");
            }

            feedback.IsHide = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Show(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                TempData["ErrorMessage"] = "Feedback not found.";
                return RedirectToAction("Index");
            }

            feedback.IsHide = false;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Manage(int equipmentId)
        {

            if (equipmentId == 0)
            {
                TempData["ErrorMessage"] = "Equipment not found.";
                return RedirectToAction("Index");
            }

            var equipment = await _context.Equipment.FindAsync(equipmentId);
            if (equipment == null)
            {
                TempData["ErrorMessage"] = "Equipment not found.";
                return RedirectToAction("Index");
            }

            ViewBag.EquipmentName = equipment.Name;

            var feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .Where(f => f.EquipmentId == equipmentId)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new FeedbackViewModel
                {
                    Id = f.Id,
                    UserName = f.User.Name,
                    Comment = f.Comment,
                    Rating = (int)f.Rating,
                    CreatedAt = (DateTime)f.CreatedAt,
                    IsHide = f.IsHide,
                    EquipmentName = f.Equipment.Name
                })
                .ToListAsync();

            return View(feedbacks);
        }


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeedbackViewModel viewModel)
        {
            // Manual check for Rating validation
            if (!viewModel.Rating.HasValue || viewModel.Rating <= 0)
            {
                ModelState.AddModelError("Rating", "Please provide a rating.");
            }

            // Remove EquipmentName from ModelState since it's not required
            ModelState.Remove("EquipmentName");

            if (!ModelState.IsValid)
            {
                // If the rating is missing, repopulate the form and return to the same view
                var equipment = await _context.Equipment.FindAsync(viewModel.EquipmentId);
                if (equipment != null)
                {
                    viewModel.EquipmentName = equipment.Name;
                }
                return View(viewModel);
            }

            var currentUserId = await GetCurrentUserIdAsync();
            if (!currentUserId.HasValue)
            {
                return Unauthorized();
            }

            try
            {
                var feedback = new Feedback
                {
                    UserId = currentUserId.Value,
                    EquipmentId = viewModel.EquipmentId,
                    Rating = viewModel.Rating.Value,
                    Comment = viewModel.Comment,
                    CreatedAt = DateTime.UtcNow,
                    IsHide = false // Default visibility
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                // Add success message
                TempData["SuccessMessage"] = "Your feedback has been submitted successfully!";

                // Redirect to equipment details
                return RedirectToAction("Details", "Equipment", new { id = viewModel.EquipmentId });
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while saving your feedback. Please try again.");

                // Repopulate the form
                var equipment = await _context.Equipment.FindAsync(viewModel.EquipmentId);
                if (equipment != null)
                {
                    viewModel.EquipmentName = equipment.Name;
                }
                return View(viewModel);
            }
        }


        private async Task<int?> GetCurrentUserIdAsync()
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
            {
                return null;
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == identityUser.Email);

            return user?.Id;
        }




    }
}
