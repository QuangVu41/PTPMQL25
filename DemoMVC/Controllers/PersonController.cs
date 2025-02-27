using DemoMVC.Data;
using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers;

public class PersonController : Controller
{

  private readonly ApplicationDbContext _context;

  public PersonController(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<IActionResult> Index()
  {
    List<Person> persons = await _context.Persons.ToListAsync();

    return View(persons);
  }

  [HttpPost]
  public IActionResult Index(Person person)
  {
    string output = $"Hello {person.PersonId} {person.FullName} {person.Address}";
    ViewBag.Message = output;
    return View();
  }

  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Create([Bind("PersonId, FullName, Address")] Person person)
  {
    // if (ModelState.IsValid)
    // {
    await _context.Persons.AddAsync(person);
    await _context.SaveChangesAsync();
    return RedirectToAction("Index");
    // }
    // return View(person);
  }

  public async Task<IActionResult> Edit(string id)
  {
    if (id == null || _context.Persons == null)
      return NotFound();

    var person = await _context.Persons.FindAsync(id);

    if (person == null) return NotFound();

    return View(person);
  }

  [HttpPost]
  public async Task<IActionResult> Edit([Bind("PersonId, FullName, Address")] Person person)
  {
    if (person.PersonId == null) return NotFound();

    if (ModelState.IsValid)
    {
      var personObj = await _context.Persons.FindAsync(person.PersonId);

      if (personObj == null) return NotFound();

      _context.Update(person);
      await _context.SaveChangesAsync();

      return RedirectToAction("Index");
    }

    return View(person);
  }

  public IActionResult PersonInfo()
  {
    return View();
  }

  public IActionResult PersonBMI()
  {
    return View();
  }

  [HttpPost]
  public IActionResult PersonBMI(Person person)
  {
    double? bmi = person.Weight / (person.Height * person.Height);
    string bmiStr = "";

    if (bmi < 18.5) bmiStr = "Underweight";
    else if (bmi < 24.9) bmiStr = "Normal";
    else if (bmi < 29.9) bmiStr = "Overweight";
    else if (bmi >= 30) bmiStr = "Obesity";

    ViewBag.Message = $"Your BMI is {bmiStr}({bmi?.ToString("0.00")})";
    return View();
  }
}