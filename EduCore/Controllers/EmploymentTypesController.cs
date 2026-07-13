using EduCore.Data;
using EduCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Controllers
{
    public class EmploymentTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmploymentTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================================================
        // INDEX
        //=========================================================
        public async Task<IActionResult> Index()
        {
            var employmentTypes = await _context.EmploymentTypes
                .OrderBy(e => e.DisplayOrder)
                .ThenBy(e => e.EmploymentTypeName)
                .ToListAsync();

            return View(employmentTypes);
        }

        //=========================================================
        // DETAILS
        //=========================================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var employmentType = await _context.EmploymentTypes
                .FirstOrDefaultAsync(e => e.EmploymentTypeId == id);

            if (employmentType == null)
                return NotFound();

            return View(employmentType);
        }

        //=========================================================
        // CREATE (GET)
        //=========================================================
        public IActionResult Create()
        {
            return View();
        }

        //=========================================================
        // CREATE (POST)
        //=========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("EmploymentTypeId,EmploymentTypeName,Description,IsActive,DisplayOrder")]
            EmploymentType employmentType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employmentType);

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Employment Type created successfully.";

                return RedirectToAction(nameof(Index));
            }

            return View(employmentType);
        }

        //=========================================================
        // EDIT (GET)
        //=========================================================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var employmentType =
                await _context.EmploymentTypes.FindAsync(id);

            if (employmentType == null)
                return NotFound();

            return View(employmentType);
        }

        //=========================================================
        // EDIT (POST)
        //=========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("EmploymentTypeId,EmploymentTypeName,Description,IsActive,DisplayOrder")]
            EmploymentType employmentType)
        {
            if (id != employmentType.EmploymentTypeId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employmentType);

                    await _context.SaveChangesAsync();

                    TempData["Success"] =
                        "Employment Type updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmploymentTypeExists(employmentType.EmploymentTypeId))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(employmentType);
        }

        //=========================================================
        // DELETE (GET)
        //=========================================================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var employmentType = await _context.EmploymentTypes
                .FirstOrDefaultAsync(e => e.EmploymentTypeId == id);

            if (employmentType == null)
                return NotFound();

            return View(employmentType);
        }

        //=========================================================
        // DELETE (POST)
        //=========================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employmentType =
                await _context.EmploymentTypes.FindAsync(id);

            if (employmentType != null)
            {
                _context.EmploymentTypes.Remove(employmentType);

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Employment Type deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        //=========================================================
        // EXISTS
        //=========================================================
        private bool EmploymentTypeExists(int id)
        {
            return _context.EmploymentTypes
                .Any(e => e.EmploymentTypeId == id);
        }
    }
}