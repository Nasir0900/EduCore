
using EduCore.Data;
using EduCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //====================================================
        // INDEX
        //====================================================
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Faculty)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(departments);
        }

        //====================================================
        // DETAILS
        //====================================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var department = await _context.Departments
                .Include(d => d.Faculty)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null)
                return NotFound();

            return View(department);
        }

        //====================================================
        // CREATE (GET)
        //====================================================
        public IActionResult Create()
        {
            ViewData["FacultyId"] = new SelectList(
                _context.Faculties.OrderBy(f => f.FacultyName),
                "FacultyId",
                "FacultyName");

            return View();
        }

        //====================================================
        // CREATE (POST)
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("DepartmentId,DepartmentName,DepartmentCode,Description,FacultyId")]
            Department department)
        {
            bool codeExists = await _context.Departments.AnyAsync(d =>
                d.DepartmentCode == department.DepartmentCode);

            if (codeExists)
            {
                ModelState.AddModelError(
                    "DepartmentCode",
                    "Department Code already exists.");
            }

            bool nameExists = await _context.Departments.AnyAsync(d =>
                d.DepartmentName == department.DepartmentName &&
                d.FacultyId == department.FacultyId);

            if (nameExists)
            {
                ModelState.AddModelError(
                    "DepartmentName",
                    "This Department already exists in the selected Faculty.");
            }

            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Department created successfully.";

                return RedirectToAction(nameof(Index));
            }

            ViewData["FacultyId"] = new SelectList(
                _context.Faculties,
                "FacultyId",
                "FacultyName",
                department.FacultyId);

            return View(department);
        }

        //====================================================
        // EDIT (GET)
        //====================================================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return NotFound();

            ViewData["FacultyId"] = new SelectList(
                _context.Faculties.OrderBy(f => f.FacultyName),
                "FacultyId",
                "FacultyName",
                department.FacultyId);

            return View(department);
        }

        //====================================================
        // EDIT (POST)
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("DepartmentId,DepartmentName,DepartmentCode,Description,FacultyId")]
            Department department)
        {
            if (id != department.DepartmentId)
                return NotFound();

            bool codeExists = await _context.Departments.AnyAsync(d =>
                d.DepartmentCode == department.DepartmentCode &&
                d.DepartmentId != department.DepartmentId);

            if (codeExists)
            {
                ModelState.AddModelError(
                    "DepartmentCode",
                    "Department Code already exists.");
            }

            bool nameExists = await _context.Departments.AnyAsync(d =>
                d.DepartmentName == department.DepartmentName &&
                d.FacultyId == department.FacultyId &&
                d.DepartmentId != department.DepartmentId);

            if (nameExists)
            {
                ModelState.AddModelError(
                    "DepartmentName",
                    "This Department already exists in the selected Faculty.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);

                    await _context.SaveChangesAsync();

                    TempData["Success"] =
                        "Department updated successfully.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DepartmentId))
                        return NotFound();

                    throw;
                }
            }

            ViewData["FacultyId"] = new SelectList(
                _context.Faculties,
                "FacultyId",
                "FacultyName",
                department.FacultyId);

            return View(department);
        }
        //====================================================
        // DELETE (GET)
        //====================================================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var department = await _context.Departments
                .Include(d => d.Faculty)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null)
                return NotFound();

            return View(department);
        }

        //====================================================
        // DELETE (POST)
        //====================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments
                .Include(d => d.AcademicPrograms)
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null)
                return NotFound();

            // Prevent deleting departments that contain Academic Programs
            if (department.AcademicPrograms.Any())
            {
                TempData["Error"] =
                    "This Department cannot be deleted because it contains Academic Programs.";

                return RedirectToAction(nameof(Index));
            }

            // Prevent deleting departments that contain Courses
            if (department.Courses.Any())
            {
                TempData["Error"] =
                    "This Department cannot be deleted because it contains Courses.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Departments.Remove(department);

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Department deleted successfully.";
            }
            catch (Exception)
            {
                TempData["Error"] =
                    "An unexpected error occurred while deleting the Department.";
            }

            return RedirectToAction(nameof(Index));
        }

        //====================================================
        // EXISTS
        //====================================================
        private bool DepartmentExists(int id)
        {
            return _context.Departments
                .Any(e => e.DepartmentId == id);
        }
    }
}