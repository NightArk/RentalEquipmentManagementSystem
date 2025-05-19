using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic;
using RentalEquipmentManagementLogic.Models;
using System;
using System.Threading.Tasks;

namespace RentalEquipmentManagementApp
{
    public class AuthService
    {
        private readonly ISharedAuthenticationService _sharedAuthService;
        private readonly EquipmentRentalDBContext _context;


        public AuthService(EquipmentRentalDBContext context)
        {
            _sharedAuthService = new SharedAuthenticationService(context);
        }

        public async Task<UserDto?> AuthenticateAsync(string email, string password)
        {
            if (!await _sharedAuthService.ValidatePasswordAsync(email, password))
                return null;

            var user = await _sharedAuthService.GetUserByEmailAsync(email);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task LogAccessAsync(int userId, string action, string affectedData)
        {
            await _sharedAuthService.LogUserActivityAsync(userId, action, affectedData, "Desktop");
        }

        public void LogAccess(int userId, string action, string affectedData)
        {
            _context.Logs.Add(new Log
            {
                UserId = userId,
                Action = action,
                Timestamp = DateTime.Now,
                AffectedData = affectedData,
                Source = "Desktop"
            });
            _context.SaveChanges();
        }
    }
}