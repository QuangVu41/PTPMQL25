using DemoMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers;

[Authorize]
public class ComponentGradesController : Controller
{
  public IActionResult Index()
  {
    return View();
  }

  [HttpPost]
  public IActionResult Index(ComponentGrades CG)
  {
    var avg = CG.C * 0.1 + CG.B * 0.3 + CG.A * 0.6;
    ViewBag.Message = $"Your average is {avg.ToString("0.00")}";
    return View();
  }
}