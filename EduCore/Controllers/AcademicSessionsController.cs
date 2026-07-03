
using EduCore.Data;
using EduCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Controllers
{
    public class AcademicSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AcademicSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AcademicSessions
        public async Task<IActionResult> Index()
        {
            var academicSessions = _context.AcademicSessions
                                           .Include(a => a.AcademicProgram);

            return View(await academicSessions.ToListAsync());
        }

        // GET: AcademicSessions/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: AcademicSessions/Create
        public IActionResult Create()
        {
            ViewData["AcademicProgramId"] = new SelectList(
                _context.AcademicPrograms,
                "AcademicProgramId",
                "ProgramName");

            return View();
        }

        // POST: AcademicSessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AcademicSessionId,SessionName,StartYear,EndYear,AcademicProgramId,IsAdmissionOpen,IsActive")] AcademicSession academicSession)
        {
            if (academicSession.EndYear < academicSession.StartYear)
            {
                ModelState.AddModelError("EndYear", "End Year cannot be less than Start Year.");
            }

            bool exists = await _context.AcademicSessions.AnyAsync(a =>
                a.AcademicProgramId == academicSession.AcademicProgramId &&
                a.StartYear == academicSession.StartYear &&
                a.EndYear == academicSession.EndYear);

            if (exists)
            {
                ModelState.AddModelError("", "This Academic Session already exists for the selected Academic Program.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(academicSession);
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
        public async Task<IActionResult> Edit(int id, [Bind("AcademicSessionId,SessionName,StartYear,EndYear,AcademicProgramId,IsAdmissionOpen,IsActive")] AcademicSession academicSession)
        {
            if (id != academicSession.AcademicSessionId)
                return NotFound();

            if (academicSession.EndYear < academicSession.StartYear)
            {
                ModelState.AddModelError("EndYear", "End Year cannot be less than Start Year.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicSessionExists(academicSession.AcademicSessionId))
                        return NotFound();

                    throw;
                }

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