
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EduCore.Data;
using EduCore.Models;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Controllers
{
    public class AcademicProgramsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AcademicProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AcademicPrograms
        public async Task<IActionResult> Index()
        {
            var programs = _context.AcademicPrograms
                                   .Include(a => a.Department);

            return View(await programs.ToListAsync());
        }

        // GET: AcademicPrograms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var academicProgram = await _context.AcademicPrograms
                .Include(a => a.Department)
                .FirstOrDefaultAsync(m => m.AcademicProgramId == id);

            if (academicProgram == null)
                return NotFound();

            return View(academicProgram);
        }

        // GET: AcademicPrograms/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName");

            return View();
        }

        // POST: AcademicPrograms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("AcademicProgramId,ProgramName,ProgramCode,Description,DepartmentId,ProgramType,DurationYears,TotalParts,TotalSemesters")]
            AcademicProgram academicProgram)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academicProgram);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName",
                academicProgram.DepartmentId);

            return View(academicProgram);
        }

        // GET: AcademicPrograms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var academicProgram = await _context.AcademicPrograms.FindAsync(id);

            if (academicProgram == null)
                return NotFound();

            ViewData["DepartmentId"] = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName",
                academicProgram.DepartmentId);

            return View(academicProgram);
        }

        // POST: AcademicPrograms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("AcademicProgramId,ProgramName,ProgramCode,Description,DepartmentId,ProgramType,DurationYears,TotalParts,TotalSemesters")]
            AcademicProgram academicProgram)
        {
            if (id != academicProgram.AcademicProgramId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicProgram);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AcademicPrograms.Any(e => e.AcademicProgramId == academicProgram.AcademicProgramId))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName",
                academicProgram.DepartmentId);

            return View(academicProgram);
        }

        // GET: AcademicPrograms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var academicProgram = await _context.AcademicPrograms
                .Include(a => a.Department)
                .FirstOrDefaultAsync(m => m.AcademicProgramId == id);

            if (academicProgram == null)
                return NotFound();

            return View(academicProgram);
        }

        // POST: AcademicPrograms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academicProgram = await _context.AcademicPrograms.FindAsync(id);

            if (academicProgram != null)
                _context.AcademicPrograms.Remove(academicProgram);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
