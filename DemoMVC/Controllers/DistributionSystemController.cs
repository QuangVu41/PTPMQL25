using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoMVC.Data;
using DemoMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace DemoMVC.Controllers
{
    [Authorize]
    public class DistributionSystemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DistributionSystemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DistributionSystem
        public async Task<IActionResult> Index()
        {
            return View(await _context.DistributionSystems.ToListAsync());
        }

        // GET: DistributionSystem/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributionSystem = await _context.DistributionSystems
                .FirstOrDefaultAsync(m => m.DistributionSystemId == id);
            if (distributionSystem == null)
            {
                return NotFound();
            }

            return View(distributionSystem);
        }

        // GET: DistributionSystem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DistributionSystem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DistributionSystemId,Name")] DistributionSystem distributionSystem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distributionSystem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(distributionSystem);
        }

        // GET: DistributionSystem/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributionSystem = await _context.DistributionSystems.FindAsync(id);
            if (distributionSystem == null)
            {
                return NotFound();
            }
            return View(distributionSystem);
        }

        // POST: DistributionSystem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DistributionSystemId,Name")] DistributionSystem distributionSystem)
        {
            if (id != distributionSystem.DistributionSystemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(distributionSystem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistributionSystemExists(distributionSystem.DistributionSystemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(distributionSystem);
        }

        // GET: DistributionSystem/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributionSystem = await _context.DistributionSystems
                .FirstOrDefaultAsync(m => m.DistributionSystemId == id);
            if (distributionSystem == null)
            {
                return NotFound();
            }

            return View(distributionSystem);
        }

        // POST: DistributionSystem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var distributionSystem = await _context.DistributionSystems.FindAsync(id);
            if (distributionSystem != null)
            {
                _context.DistributionSystems.Remove(distributionSystem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistributionSystemExists(string id)
        {
            return _context.DistributionSystems.Any(e => e.DistributionSystemId == id);
        }
    }
}
