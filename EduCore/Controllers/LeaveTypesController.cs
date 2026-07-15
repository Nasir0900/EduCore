
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduCore.Models;
using EduCore.Data;

public class LeaveTypesController : Controller
{
    private readonly ApplicationDbContext _context;

    public LeaveTypesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: LEAVETYPES
    //=========================================================
    // INDEX
    //=========================================================
    public async Task<IActionResult> Index()
    {
        var leaveTypes = await _context.LeaveTypes
            .OrderBy(l => l.DisplayOrder)
            .ThenBy(l => l.LeaveTypeName)
            .ToListAsync();

        return View(leaveTypes);
    }

    // GET: LEAVETYPES/Details/5
    public async Task<IActionResult> Details(int? leavetypeid)
    {
        if (leavetypeid == null)
        {
            return NotFound();
        }

        var leavetype = await _context.LeaveTypes
            .FirstOrDefaultAsync(m => m.LeaveTypeId == leavetypeid);
        if (leavetype == null)
        {
            return NotFound();
        }

        return View(leavetype);
    }

    // GET: LEAVETYPES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: LEAVETYPES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LeaveTypeId,LeaveTypeName,LeaveCode,Description,MaximumDays,RequiresApproval,RequiresDocuments,AllowCarryForward,AllowBackDateApplication,AvailableAfterMonths,IsYearlyLimit,IsPaidLeave,DisplayOrder,IsActive,CreatedDate,EmployeeLeaves")] LeaveType leavetype)
    {
        if (ModelState.IsValid)
        {
            _context.LeaveTypes.Add(leavetype);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Leave Type created successfully.";

            return RedirectToAction(nameof(Index));
        }
        return View(leavetype);
    }

    // GET: LEAVETYPES/Edit/5
    public async Task<IActionResult> Edit(int? leavetypeid)
    {
        if (leavetypeid == null)
        {
            return NotFound();
        }

        var leavetype = await _context.LeaveTypes.FindAsync(leavetypeid);
        if (leavetype == null)
        {
            return NotFound();
        }
        return View(leavetype);
    }

    // POST: LEAVETYPES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? leavetypeid, [Bind("LeaveTypeId,LeaveTypeName,LeaveCode,Description,MaximumDays,RequiresApproval,RequiresDocuments,AllowCarryForward,AllowBackDateApplication,AvailableAfterMonths,IsYearlyLimit,IsPaidLeave,DisplayOrder,IsActive,CreatedDate,EmployeeLeaves")] LeaveType leavetype)
    {
        if (leavetypeid != leavetype.LeaveTypeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(leavetype);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveTypeExists(leavetype.LeaveTypeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["Success"] =
    "Leave Type updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        return View(leavetype);
    }

    // GET: LEAVETYPES/Delete/5
    public async Task<IActionResult> Delete(int? leavetypeid)
    {
        if (leavetypeid == null)
        {
            return NotFound();
        }

        var leavetype = await _context.LeaveTypes
            .FirstOrDefaultAsync(m => m.LeaveTypeId == leavetypeid);
        if (leavetype == null)
        {
            return NotFound();
        }

        return View(leavetype);
    }

    // POST: LEAVETYPES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? leavetypeid)
    {
        var leavetype = await _context.LeaveTypes.FindAsync(leavetypeid);
        if (leavetype != null)
        {
            _context.LeaveTypes.Remove(leavetype);
        }

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Leave Type deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    private bool LeaveTypeExists(int? leavetypeid)
    {
        return _context.LeaveTypes.Any(e => e.LeaveTypeId == leavetypeid);
    }
}
