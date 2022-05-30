using Microsoft.EntityFrameworkCore;
using PapaMobileX.Server.DataSource.Models;

namespace PapaMobileX.Server.DataSource;

public class CommonContext : DbContext
{
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Claim> Claims { get; set; } = null!;

    public CommonContext(DbContextOptions<CommonContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Account>()
            .HasIndex(a => a.UserName)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}