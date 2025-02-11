using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers;

public class PersonController : Controller {
  public IActionResult Index() {
    return View();
  }

  [HttpPost]
  public IActionResult Index(Person person) {
    string output = $"Hello {person.PersonId} {person.FullName} {person.Address}";
    ViewBag.Message = output;
    return View();
  }

  public IActionResult PersonInfo() {
    return View();
  }
}