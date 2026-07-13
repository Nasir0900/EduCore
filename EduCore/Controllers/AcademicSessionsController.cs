using EduCore.Data;
using EduCore.Interfaces;
using EduCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Controllers
{
    public class AcademicSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAcademicStructureService _academicStructureService;

        public AcademicSessionsController(
            ApplicationDbContext context,
            IAcademicStructureService academicStructureService)
        {
            _context = context;
            _academicStructureService = academicStructureService;
        }

        // GET: AcademicSessions
        public async Task<IActionResult> Index()
        {
            var academicSessions = await _context.AcademicSessions
                .Include(a => a.AcademicProgram)
                .Include(a => a.Parts)
                .ToListAsync();

            return View(academicSessions);
        }

        // GET: AcademicSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var academicSession = await _context.AcademicSessions
                .Include(a => a.AcademicProgram)
                .Include(a => a.Parts)
                    .ThenInclude(p => p.Semesters)
                .FirstOrDefaultAsync(m => m.AcademicSessionId == id);

            if (academicSession == null)
                return NotFound();

            ViewBag.StructureGenerated =
                await _academicStructureService.StructureExistsAsync(academicSession.AcademicSessionId);

            return View(academicSession);
        }

        // POST: AcademicSessions/GenerateStructure/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateStructure(int id)
        {
            try
            {
                await _academicStructureService.GenerateAcademicStructureAsync(id);

                TempData["Success"] = "Academic Structure generated successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: AcademicSessions/Create
        public IActionResult Create()
        {
            ViewBag.AcademicPrograms = _context.AcademicPrograms
                .Select(p => new
                {
                    p.AcademicProgramId,
                    p.ProgramName,
                    p.DurationYears
                })
                .ToList();

            ViewData["AcademicProgramId"] = new SelectList(
                _context.AcademicPrograms,
                "AcademicProgramId",
                "ProgramName");

            return View();
        }

        // POST: AcademicSessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AcademicSessionId,StartYear,AcademicProgramId,IsAdmissionOpen,IsActive")] AcademicSession academicSession)
        {
            var program = await _context.AcademicPrograms
                .FirstOrDefaultAsync(p => p.AcademicProgramId == academicSession.AcademicProgramId);

            if (program == null)
            {
                ModelState.AddModelError("", "Invalid Academic Program.");
            }
            else
            {
                academicSession.EndYear = academicSession.StartYear + program.DurationYears - 1;
                academicSession.SessionName = $"{academicSession.StartYear}-{academicSession.EndYear}";
            }

            bool exists = await _context.AcademicSessions.AnyAsync(a =>
                a.AcademicProgramId == academicSession.AcademicProgramId &&
                a.StartYear == academicSession.StartYear);

            if (exists)
            {
                ModelState.AddModelError("", "This Academic Session already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.AcademicSessions.Add(academicSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AcademicProgramId"] = new SelectList(
                _context.AcademicPrograms,
                "AcademicProgramId",
                "ProgramName",
                academicSession.AcademicProgramId);

            return View(academicSession);
        }

        // GET: AcademicSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var academicSession = await _context.AcademicSessions.FindAsync(id);

            if (academicSession == null)
                return NotFound();

            ViewBag.AcademicPrograms = _context.AcademicPrograms
    .Select(p => new
    {
        p.AcademicProgramId,
        p.ProgramName,
        p.DurationYears
    })
    .ToList();

            ViewData["AcademicProgramId"] = new SelectList(
                _context.AcademicPrograms,
                "AcademicProgramId",
                "ProgramName",
                academicSession.AcademicProgramId);

            return View(academicSession);
        }

        // POST: AcademicSessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
     [Bind("AcademicSessionId,StartYear,AcademicProgramId,IsAdmissionOpen,IsActive")]
    AcademicSession academicSession)
        {
            if (id != academicSession.AcademicSessionId)
                return NotFound();

            var existingSession = await _context.AcademicSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AcademicSessionId == id);

            if (existingSession == null)
                return NotFound();

            var program = await _context.AcademicPrograms
                .FirstOrDefaultAsync(p => p.AcademicProgramId == academicSession.AcademicProgramId);

            if (program == null)
            {
                ModelState.AddModelError("", "Invalid Academic Program.");
            }
            else
            {
                academicSession.EndYear = academicSession.StartYear + program.DurationYears - 1;
                academicSession.SessionName = $"{academicSession.StartYear}-{academicSession.EndYear}";
                academicSession.CreatedDate = existingSession.CreatedDate;
            }

            if (ModelState.IsValid)
            {
                _context.Update(academicSession);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["AcademicProgramId"] = new SelectList(
                _context.AcademicPrograms,
                "AcademicProgramId",
                "ProgramName",
                academicSession.AcademicProgramId);

            return View(academicSession);
        }

        // GET: AcademicSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var academicSession = await _context.AcademicSessions
                .Include(a => a.AcademicProgram)
                .FirstOrDefaultAsync(m => m.AcademicSessionId == id);

            if (academicSession == null)
                return NotFound();

            return View(academicSession);
        }

        // POST: AcademicSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academicSession = await _context.AcademicSessions.FindAsync(id);

            if (academicSession != null)
                _context.AcademicSessions.Remove(academicSession);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool AcademicSessionExists(int id)
        {
            return _context.AcademicSessions.Any(e => e.AcademicSessionId == id);
        }
    }
}