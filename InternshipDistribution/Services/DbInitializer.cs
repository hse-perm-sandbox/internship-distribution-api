using InternshipDistribution.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternshipDistribution.Services
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, BCryptPasswordHasher hasher)
        {
            context.Database.Migrate();

            var email = Environment.GetEnvironmentVariable("DEFAULT_MANAGER_EMAIL");
            var password = Environment.GetEnvironmentVariable("DEFAULT_MANAGER_PASSWORD");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new NullReferenceException($"email или password пустые или не найдены");

            if (!context.Users.Any(u => u.Email == email))
            {
                context.Users.Add(new User
                {
                    Email = email,
                    PasswordHash = hasher.HashPassword(password),
                    IsManager = true,
                    CreatedAt = DateTime.UtcNow
                });

                context.SaveChanges();
            }
        }
    }
}
