using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;

namespace RentalEquipmentManagementWebApp.Data
{
    public static class SeedData
    {
        public static async Task Initialize(EquipmentRentalDBContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create roles if they don't exist
            string[] roles = { "Administrator", "Manager", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create admin user if it doesn't exist
            var adminEmail = "admin@rentalequipment.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");

                    // Create corresponding user in our custom User table
                    var user = new User
                    {
                        Name = "System Administrator",
                        Email = adminEmail,
                        PasswordHash = userManager.PasswordHasher.HashPassword(adminUser, "Admin@123"),
                        Role = "Administrator",
                        CreatedAt = DateTime.Now
                    };

                    context.Users.Add(user);
                    await context.SaveChangesAsync();
                }
            }

            // Seed categories if they don't exist
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Heavy Machinery", Description = "Large construction equipment like excavators, bulldozers, etc." },
                    new Category { Name = "Power Tools", Description = "Handheld and portable power tools for construction and DIY projects" },
                    new Category { Name = "Gardening Equipment", Description = "Tools and equipment for gardening and landscaping" },
                    new Category { Name = "Event Equipment", Description = "Equipment for events, parties, and gatherings" },
                    new Category { Name = "Audio/Visual Equipment", Description = "Sound systems, projectors, screens, and other A/V equipment" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Seed sample equipment if they don't exist
            if (!context.Equipment.Any())
            {
                var categories = await context.Categories.ToListAsync();
                var equipmentList = new List<Equipment>();

                foreach (var category in categories)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        equipmentList.Add(new Equipment
                        {
                            Name = $"{category.Name} Item {i}",
                            Description = $"Sample {category.Name.ToLower()} equipment for rental",
                            CategoryId = category.Id,
                            AvailabilityStatus = "Available",
                            ConditionStatus = "Excellent",
                            RentalPrice = (decimal)(50 + new Random().Next(10, 200)),
                            CreatedAt = DateTime.Now
                        });
                    }
                }

                context.Equipment.AddRange(equipmentList);
                await context.SaveChangesAsync();
            }
        }
    }
}
