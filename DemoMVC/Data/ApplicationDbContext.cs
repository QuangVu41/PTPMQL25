using DemoMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DemoMVC.Models.Entities;

namespace DemoMVC.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
  public DbSet<Person> Persons { get; set; }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Agency> Agencies { get; set; }
  public DbSet<DistributionSystem> DistributionSystems { get; set; }
  public DbSet<Worker> Workers { get; set; }
  public DbSet<MemberUnit> MemberUnits { get; set; }
  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    builder.Entity<ApplicationUser>().ToTable("Users");
    builder.Entity<IdentityRole>().ToTable("Roles");
    builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
    builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
    builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
    builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
    builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
  }

  public DbSet<DemoMVC.Models.Entities.Worker> Worker { get; set; } = default!;

  public DbSet<DemoMVC.Models.Entities.MemberUnit> MemberUnit { get; set; } = default!;
}