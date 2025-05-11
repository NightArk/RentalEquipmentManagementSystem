using RentalEquipmentManagementLogic;
using RentalEquipmentManagementLogic.Models;
using System;
using System.Threading.Tasks;

namespace RentalEquipmentManagementApp
{
    public class AuthService
    {
        private readonly ISharedAuthenticationService _sharedAuthService;

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
    }
}