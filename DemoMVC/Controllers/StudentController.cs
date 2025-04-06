using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers;
public class StudentController : Controller
{
  public IActionResult Index()
  {
    Student student = new Student() { Id = "qv413", Name = "Vu Quang" };

    return View(student);
  }

  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public IActionResult Create(Student std)
  {
    ViewBag.Message = $"My name is {std.Name}, Id: {std.Id}";
    return View();
  }
}