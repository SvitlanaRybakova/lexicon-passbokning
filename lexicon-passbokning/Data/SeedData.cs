using lexicon_passbokning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace lexicon_passbokning.Data
{
    public class SeedData
    {
        private static ApplicationDbContext context = default!;
        private static RoleManager<IdentityRole> roleManager = default!;
        private static UserManager<ApplicationUser> userManager = default!;


        public static async Task Init(ApplicationDbContext _context, IServiceProvider services)
        {
            context = _context;


            if (context.Roles.Any()) return;

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            var roleNames = new[] { "User", "Admin" };
            var adminEmail = "admin@Gymbokning.se";
            var userEmail = "user@user.com";

            await AddRolesAsync(roleNames);

            var admin = await AddAccountAsync(adminEmail, "Admin", "Adminsson", "PWadmin-123");
            var user = await AddAccountAsync(userEmail, "User", "Usersson", "PWuser-123");

            await AddUserToRoleAsync(admin, "Admin");
            await AddUserToRoleAsync(user, "User");

            await AddClassAsync(context, "Zumba", new DateTime(2024, 12, 15, 9, 0, 0), TimeSpan.FromMinutes(60), "A beginner - friendly zumba class focusing on basic postures and relaxation.");
            await AddClassAsync(context, "Yoga", new DateTime(2024, 11, 15, 9, 0, 0), TimeSpan.FromMinutes(45), "A beginner - friendly zumba class focusing on basic postures and relaxation.");
            await AddClassAsync(context, "HIIT Workout", new DateTime(2023, 11, 15, 18, 30, 0), TimeSpan.FromMinutes(45), "A high-intensity interval training (HIIT) class designed to burn calories and build endurance.");


        }
        private static async Task AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }
        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }
        private static async Task<ApplicationUser> AddAccountAsync(string accountEmail, string fName, string lName, string pw)
        {
            var found = await userManager.FindByEmailAsync(accountEmail);
            if (found != null) return null!;

            var user = new ApplicationUser
            {
                UserName = accountEmail,
                Email = accountEmail,
                FirstName = fName,
                LastName = lName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, pw);

            if (!result.Succeeded)
            {
                Console.WriteLine($"Error during seeding: {string.Join("\n", result.Errors)}");
                throw new Exception(string.Join("\n", result.Errors));
               
            }
                
            return user;
        }

        private static async Task<GymClass> AddClassAsync(ApplicationDbContext _context, string className, DateTime classStartTime, TimeSpan classDuration, string classDescription)
        {
            // Create a new GymClass entity
            var gymClass = new GymClass
            {
                Name = className,
                StartTime = classStartTime,
                Duration = classDuration,
                Description = classDescription
            };

            // Add the GymClass entity to the DbContext
            await _context.GymClasses.AddAsync(gymClass);

            // Save the changes to the database
            var result = await _context.SaveChangesAsync();

            // Check if the save was successful
            if (result == 0)
            {
                Console.WriteLine("Error during seeding: Failed to add class to the database.");
                throw new Exception("Failed to add class to the database.");
            }

            return gymClass; // Return the created gym class
        }


    }
}
