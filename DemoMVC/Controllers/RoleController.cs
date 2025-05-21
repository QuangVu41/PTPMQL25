using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers;

public class RoleController : Controller
{
  private readonly RoleManager<IdentityRole> _roleManager;
  public RoleController(RoleManager<IdentityRole> roleManager)
  {
    _roleManager = roleManager;
  }
  public IActionResult Index()
  {
    var roles = _roleManager.Roles.ToList();
    return View(roles);
  }

  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Create(string roleName)
  {
    if (!string.IsNullOrEmpty(roleName))
    {
      var role = new IdentityRole(roleName.Trim());
      await _roleManager.CreateAsync(role);
    }
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Edit(string id)
  {
    var role = await _roleManager.FindByIdAsync(id);
    if (role == null)
    {
      return NotFound();
    }
    return View(role);
  }
  [HttpPost]
  public async Task<IActionResult> Edit(string id, string newName)
  {
    var role = await _roleManager.FindByIdAsync(id);
    if (role == null)
    {
      return NotFound();
    }
    role.Name = newName;
    await _roleManager.UpdateAsync(role);
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Delete(string id)
  {
    var role = await _roleManager.FindByIdAsync(id);
    if (role == null)
    {
      return NotFound();
    }
    return View(role);
  }

  [HttpPost]
  [ActionName("Delete")]
  public async Task<IActionResult> DeleteRole(string id)
  {
    var role = await _roleManager.FindByIdAsync(id);
    if (role != null)
    {
      await _roleManager.DeleteAsync(role);
    }
    return RedirectToAction("Index");
  }
}