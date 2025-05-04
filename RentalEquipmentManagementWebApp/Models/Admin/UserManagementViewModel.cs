namespace RentalEquipmentManagementWebApp.Models.Admin
{
    public class UserManagementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? IdentityUserId { get; set; }
    }

    public class EditUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? IdentityUserId { get; set; }
        public bool ResetPassword { get; set; }
    }
}
