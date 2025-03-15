using DemoMVC.Data;
using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

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

  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Create([Bind("PersonId, FullName, Address, Height, Weight")] Person person)
  {
    await _context.Persons.AddAsync(person);
    await _context.SaveChangesAsync();
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Edit(int id)
  {
    if (id == 0 || _context.Persons == null)
      return NotFound();

    var person = await _context.Persons.FindAsync(id);

    if (person == null) return NotFound();

    return View(person);
  }

  [HttpPost]
  public async Task<IActionResult> Edit(int id, [Bind("PersonId, FullName, Address, Height, Weight")] Person person)
  {
    if (person.PersonId == 0) return NotFound();

    if (!PersonExists(person.PersonId)) return NotFound();

    _context.Update(person);
    await _context.SaveChangesAsync();

    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Delete(int id)
  {
    if (id == 0 || _context.Persons == null)
      return NotFound();

    var person = await _context.Persons.FindAsync(id);

    if (person == null) return NotFound();

    return View(person);
  }

  [HttpPost, ActionName("Delete")]
  public async Task<IActionResult> DeleteAction(int id)
  {
    if (_context.Persons == null) return Problem("Have no users in the database");

    var person = await _context.Persons.FindAsync(id);

    if (person != null) _context.Persons.Remove(person);

    await _context.SaveChangesAsync();

    return RedirectToAction("Index");
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

  public async Task<IActionResult> Upload()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Upload(IFormFile file)
  {
    if (file != null)
    {
      string fileExt = Path.GetExtension(file.FileName);
      if (fileExt != ".xls" || fileExt != ".xlsx")
      {
        ModelState.AddModelError("", "Excel file is invalid!");
      }
      else
      {
        var fileName = DateTime.Now.ToShortTimeString() + fileExt;
        var filePath = Path.Combine($"{Directory.GetCurrentDirectory()}/Uploads/Excels", fileName);
        var fileLocation = new FileInfo(filePath).ToString();
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await file.CopyToAsync(stream);
        }
        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
          ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
          var dataTable = worksheet.Cells["A1:C11"].ToDataTable();
        }
      }
    }
    return View();
  }

  private bool PersonExists(int id)
  {
    return (_context.Persons?.Any(p => p.PersonId == id)).GetValueOrDefault();
  }
}