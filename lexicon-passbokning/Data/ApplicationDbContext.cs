using lexicon_passbokning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lexicon_passbokning.Data
{
    // the contex config to use the correct types for users and roles.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GymClass> GymClasses{ get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserGymClass>()
                .HasKey(t => new { t.ApplicationUserId, t.GymClassId });

            // Configure TimeOfRegistration as a shadow property for ApplicationUser
            modelBuilder.Entity<ApplicationUser>()
                .Property<DateTime>("TimeOfRegistration");
        }

        // set the current data/time when the application user table has been created (just once!)
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries<ApplicationUser>().Where(e => e.State == EntityState.Added))
            {
                entry.Property("TimeOfRegistration").CurrentValue = DateTime.Now;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
