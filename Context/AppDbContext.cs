using backEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}