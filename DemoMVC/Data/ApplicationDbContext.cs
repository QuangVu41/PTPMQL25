using DemoMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

  public DbSet<Person> Persons { get; set; }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Agency> Agencies { get; set; }
  public DbSet<DistributionSystem> DistributionSystems { get; set; }
}