
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduCore.Models;
using EduCore.Data;

public class DesignationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DesignationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: DESIGNATIONS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Designations.ToListAsync());
    }

    // GET: DESIGNATIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var designation = await _context.Designations
            .FirstOrDefaultAsync(m => m.DesignationId == id);
        if (designation == null)
        {
            return NotFound();
        }

        return View(designation);
    }

    // GET: DESIGNATIONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DESIGNATIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DesignationId,DesignationName,ShortName,IsTeaching,DisplayOrder,IsActive,CreatedDate")] Designation designation)
    {
        if (ModelState.IsValid)
        {
            _context.Add(designation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(designation);
    }

    // GET: DESIGNATIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var designation = await _context.Designations.FindAsync(id);
        if (designation == null)
        {
            return NotFound();
        }
        return View(designation);
    }

    // POST: DESIGNATIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("DesignationId,DesignationName,ShortName,IsTeaching,DisplayOrder,IsActive,CreatedDate")] Designation designation)
    {
        if (id != designation.DesignationId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(designation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DesignationExists(designation.DesignationId))
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
        return View(designation);
    }

    // GET: DESIGNATIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var designation = await _context.Designations
            .FirstOrDefaultAsync(m => m.DesignationId == id);
        if (designation == null)
        {
            return NotFound();
        }

        return View(designation);
    }

    // POST: DESIGNATIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var designation = await _context.Designations.FindAsync(id);
        if (designation != null)
        {
            _context.Designations.Remove(designation);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DesignationExists(int? designationid)
    {
        return _context.Designations.Any(e => e.DesignationId == designationid);
    }
}
