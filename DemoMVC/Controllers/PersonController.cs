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

  public IActionResult PersonBMI() {
    return View();
  }

  [HttpPost]
  public IActionResult PersonBMI(Person person) {
    double bmi = person.Weight / (person.Height * person.Height);
    string bmiStr = "";

    if (bmi < 18.5) bmiStr = "Underweight";
    else if (bmi < 24.9) bmiStr = "Normal";
    else if (bmi < 29.9) bmiStr = "Overweight";
    else if (bmi >= 30) bmiStr = "Obesity";

    ViewBag.Message = $"Your BMI is {bmiStr}({bmi.ToString("0.00")})";
    return View();
  }
}