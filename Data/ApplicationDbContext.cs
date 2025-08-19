using Elagy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elagy.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Report> reports { get; set; }
        public DbSet<Pharmacy> pharmacies { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Roshta> roshtat { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "2",
                Name = "Pharmacy",
                NormalizedName = "PHARMACY"
            });

            builder.Entity<User>().Property(x => x.UserName).HasDefaultValue(null);

            base.OnModelCreating(builder);
        }
    }
}
