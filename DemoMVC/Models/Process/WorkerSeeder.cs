using Bogus;
using DemoMVC.Data;
using DemoMVC.Models.Entities;
namespace DemoMVC.Models.Process
{
  public class WorkerSeeder
  {
    private readonly ApplicationDbContext _context;
    public WorkerSeeder(ApplicationDbContext context)
    {
      _context = context;
    }
    public void SeedWorkers(int n)
    {
      var Workers = GenerateWorkers(n);
      _context.Worker.AddRange(Workers);
      _context.SaveChanges();
    }
    private List<Worker> GenerateWorkers(int n)
    {
      var faker = new Faker<Worker>()
          .RuleFor(e => e.FirstName, f => f.Name.FirstName())
          .RuleFor(e => e.LastName, f => f.Name.LastName())
          .RuleFor(e => e.Address, f => f.Address.FullAddress())
          .RuleFor(e => e.DateOfBirth, f => f.Date.Past(30, DateTime.Now.AddYears(-18).ToUniversalTime()))
          .RuleFor(e => e.Position, f => f.Name.JobTitle())
          .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName))
          .RuleFor(e => e.HireDate, f => f.Date.Past(10).ToUniversalTime());
      return faker.Generate(n);
    }
    public async Task SeedAsync(int numberOfWorkers)
    {
      if (numberOfWorkers <= 0) throw new ArgumentException("Number of Workers must be greater than zero.");

      var faker = new Faker<Worker>()
          .RuleFor(e => e.FirstName, f => f.Name.FirstName())
          .RuleFor(e => e.LastName, f => f.Name.LastName())
          .RuleFor(e => e.Address, f => f.Address.FullAddress())
          .RuleFor(e => e.DateOfBirth, f => f.Date.Past(30, DateTime.Now.AddYears(-18).ToUniversalTime()))
          .RuleFor(e => e.Position, f => f.Name.JobTitle())
          .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName))
          .RuleFor(e => e.HireDate, f => f.Date.Past(10).ToUniversalTime());
      var Workers = faker.Generate(numberOfWorkers);
      _context.Worker.AddRange(Workers);
      await _context.SaveChangesAsync();
    }
  }
}

