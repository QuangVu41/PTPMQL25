using DemoMVC.Data;
using DemoMVC.Models;
using DemoMVC.Models.Process;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace DemoMVC.Controllers;
[Authorize]
public class PersonController : Controller
{
  private readonly ExcelProcess _excelProcess = new ExcelProcess();

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
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Upload(IFormFile file)
  {
    if (file == null)
    {
      ModelState.AddModelError("Model", "Please select an Excel file!");
      return View();
    }
    if (file != null)
    {
      string fileExt = Path.GetExtension(file.FileName);
      if (fileExt != ".xls" && fileExt != ".xlsx")
      {
        ModelState.AddModelError("", "Excel file is invalid!");
      }
      else
      {
        var fileName = "persons-" + DateTime.Now.Millisecond + fileExt;
        Console.WriteLine(fileName);
        var filePath = Path.Combine($"{Directory.GetCurrentDirectory()}/Uploads/Excels", fileName);
        var fileLocation = new FileInfo(filePath).ToString();
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await file.CopyToAsync(stream);

          var dt = _excelProcess.ExcelToDataTable(fileLocation);

          for (int i = 0; i < dt.Rows.Count; i++)
          {
            var person = new Person();
            person.PersonId = Convert.ToInt32(dt.Rows[i][0]);
            person.FullName = dt.Rows[i][1].ToString();
            person.Address = dt.Rows[i][2].ToString();
            person.Height = Convert.ToDouble(dt.Rows[i][3]);
            person.Weight = Convert.ToDouble(dt.Rows[i][4]);
            _context.Add(person);
          }
          await _context.SaveChangesAsync();
          return RedirectToAction("Index");
        }
      }
    }

    return View();
  }

  public IActionResult Download()
  {
    var fileName = "persons-" + DateTime.Now.Millisecond + ".xlsx";
    using (ExcelPackage excelPackage = new ExcelPackage())
    {
      ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Persons");
      worksheet.Cells["A1"].Value = "PersonId";
      worksheet.Cells["B1"].Value = "FullName";
      worksheet.Cells["C1"].Value = "Address";
      worksheet.Cells["D1"].Value = "Height";
      worksheet.Cells["E1"].Value = "Weight";

      var persons = _context.Persons.ToList();
      worksheet.Cells["A2"].LoadFromCollection(persons);

      var stream = new MemoryStream(excelPackage.GetAsByteArray());
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
  }

  private bool PersonExists(int id)
  {
    return (_context.Persons?.Any(p => p.PersonId == id)).GetValueOrDefault();
  }
}