using EDMS.DSM.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EDMS.DSM.Server.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //public DbSet<Communications> Communications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        
        builder.Entity<Communications>().HasNoKey();

        //_ = builder.Entity<ApplicationUser>()
        //.ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
    }
}
