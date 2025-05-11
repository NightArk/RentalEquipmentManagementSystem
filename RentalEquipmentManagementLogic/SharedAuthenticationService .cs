using Microsoft.AspNetCore.Identity;
using RentalEquipmentManagementLogic.Models;


namespace RentalEquipmentManagementLogic
{
    public interface ISharedAuthenticationService
    {
        Task<bool> ValidatePasswordAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task<string> HashPasswordAsync(string password, User user);
        Task LogUserActivityAsync(int userId, string action, string details, string source);
    }

    public class SharedAuthenticationService : ISharedAuthenticationService
    {
        private readonly EquipmentRentalDBContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;

        public SharedAuthenticationService(EquipmentRentalDBContext dbContext)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<bool> ValidatePasswordAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null) return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result != PasswordVerificationResult.Failed;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await Task.FromResult(_dbContext.Users.FirstOrDefault(u => u.Email == email));
        }

        public async Task<string> HashPasswordAsync(string password, User user)
        {
            return await Task.FromResult(_passwordHasher.HashPassword(user, password));
        }

        public async Task LogUserActivityAsync(int userId, string action, string details, string source)
        {
            _dbContext.Logs.Add(new Log
            {
                UserId = userId,
                Action = action,
                Timestamp = DateTime.Now,
                AffectedData = details,
                Source = source
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}
