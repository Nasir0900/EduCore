
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduCore.Models;
using EduCore.Data;

namespace EduCore.Controllers
{
    public class FacultiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacultiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FACULTYS
        public async Task<IActionResult> Index()
        {
            return View(await _context.Faculties.ToListAsync());
        }

        // GET: FACULTYS/Details/5
        public async Task<IActionResult> Details(int? facultyid)
        {
            if (facultyid == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(m => m.FacultyId == facultyid);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // GET: FACULTYS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FACULTYS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FacultyId,FacultyName,Description,CreatedDate")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faculty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: FACULTYS/Edit/5
        public async Task<IActionResult> Edit(int? facultyid)
        {
            if (facultyid == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties.FindAsync(facultyid);
            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }

        // POST: FACULTYS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? facultyid, [Bind("FacultyId,FacultyName,Description,CreatedDate")] Faculty faculty)
        {
            if (facultyid != faculty.FacultyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faculty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyExists(faculty.FacultyId))
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
            return View(faculty);
        }

        // GET: FACULTYS/Delete/5
        public async Task<IActionResult> Delete(int? facultyid)
        {
            if (facultyid == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(m => m.FacultyId == facultyid);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // POST: FACULTYS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? facultyid)
        {
            var faculty = await _context.Faculties.FindAsync(facultyid);
            if (faculty != null)
            {
                _context.Faculties.Remove(faculty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacultyExists(int? facultyid)
        {
            return _context.Faculties.Any(e => e.FacultyId == facultyid);
        }
    }
}
