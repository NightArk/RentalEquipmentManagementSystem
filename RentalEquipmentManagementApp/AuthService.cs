using RentalEquipmentManagementLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RentalEquipmentManagementApp
{
    public class AuthService
    {
        private readonly EquipmentRentalDBContext _context;

        public AuthService(EquipmentRentalDBContext context)
        {
            _context = context;
        }

        public User? Authenticate(string email, string password)
        {
            return _context.Users.FirstOrDefault(u =>
                u.Email == email &&
                u.PasswordHash == password); // In production, use hashed passwords
        }

        public void LogAccess(int userId, string action)
        {
            _context.Logs.Add(new Log
            {
                UserId = userId,
                Action = action,
                Timestamp = DateTime.Now,
                Source = "Desktop"
            });
            _context.SaveChanges();
        }
    }
}
