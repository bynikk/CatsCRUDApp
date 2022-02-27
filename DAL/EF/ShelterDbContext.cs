using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class ShelterDbContext : DbContext
    {
        public DbSet<Cat> Cats { get; set; }
        public DbSet<Order> Orders { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ShelterDbContext(DbContextOptions<ShelterDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
