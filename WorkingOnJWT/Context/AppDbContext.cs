using Microsoft.EntityFrameworkCore;
using WorkingOnJWT.Entities;

namespace WorkingOnJWT.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
}