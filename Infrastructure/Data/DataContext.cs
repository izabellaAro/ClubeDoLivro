using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data;

public class DataContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookShelf> BooksShelf { get; set; }
    public DbSet<ReadingSchedule> ReadingSchedules { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}