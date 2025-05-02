using RentalEquipmentManagementLogic;
using RentalEquipmentManagementLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace RentalEquipmentManagementApp
{
    public class AuthService
    {
        private readonly EquipmentRentalDBContext _context;

        public AuthService(EquipmentRentalDBContext context)
        {
            _context = context;
        }

        public UserDto? Authenticate(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return null;

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
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
