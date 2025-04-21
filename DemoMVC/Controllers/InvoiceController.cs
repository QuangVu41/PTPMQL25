using DemoMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers;

[Authorize]
public class InvoiceController : Controller
{
  public IActionResult Index()
  {
    return View();
  }

  [HttpPost]
  public IActionResult Index(Invoice invoice)
  {
    double totalPrice = invoice.Price * invoice.Quantity;
    ViewBag.Message = $"Your total price is {totalPrice.ToString("0.00")}";
    return View();
  }
}