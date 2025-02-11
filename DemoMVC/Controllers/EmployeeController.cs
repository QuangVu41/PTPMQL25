using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers;

public class EmployeeController : Controller {
  public IActionResult Index() {
    return View();
  }

  public IActionResult EmployeeInfo() {
    return View();
  }
}