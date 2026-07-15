
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduCore.Models;
using EduCore.Data;

public class EmployeeLeavesController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeeLeavesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: EMPLOYEELEAVES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.EmployeeLeaves.ToListAsync());
    }

    // GET: EMPLOYEELEAVES/Details/5
    public async Task<IActionResult> Details(int? employeeleaveid)
    {
        if (employeeleaveid == null)
        {
            return NotFound();
        }

        var employeeleave = await _context.EmployeeLeaves
            .FirstOrDefaultAsync(m => m.EmployeeLeaveId == employeeleaveid);
        if (employeeleave == null)
        {
            return NotFound();
        }

        return View(employeeleave);
    }

    // GET: EMPLOYEELEAVES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: EMPLOYEELEAVES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EmployeeLeaveId,LeaveNumber,EmployeeId,Employee,LeaveTypeId,LeaveType,FromDate,ToDate,TotalDays,Reason,IsHalfDay,AttachmentPath,ExpectedRejoiningDate,ActualRejoiningDate,Status,ApprovedByEmployeeId,ApprovedBy,ApprovalDate,ApprovalRemarks,IsCancelled,CancelledDate,AppliedDate")] EmployeeLeave employeeleave)
    {
        if (ModelState.IsValid)
        {
            _context.Add(employeeleave);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(employeeleave);
    }

    // GET: EMPLOYEELEAVES/Edit/5
    public async Task<IActionResult> Edit(int? employeeleaveid)
    {
        if (employeeleaveid == null)
        {
            return NotFound();
        }

        var employeeleave = await _context.EmployeeLeaves.FindAsync(employeeleaveid);
        if (employeeleave == null)
        {
            return NotFound();
        }
        return View(employeeleave);
    }

    // POST: EMPLOYEELEAVES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? employeeleaveid, [Bind("EmployeeLeaveId,LeaveNumber,EmployeeId,Employee,LeaveTypeId,LeaveType,FromDate,ToDate,TotalDays,Reason,IsHalfDay,AttachmentPath,ExpectedRejoiningDate,ActualRejoiningDate,Status,ApprovedByEmployeeId,ApprovedBy,ApprovalDate,ApprovalRemarks,IsCancelled,CancelledDate,AppliedDate")] EmployeeLeave employeeleave)
    {
        if (employeeleaveid != employeeleave.EmployeeLeaveId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(employeeleave);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeLeaveExists(employeeleave.EmployeeLeaveId))
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
        return View(employeeleave);
    }

    // GET: EMPLOYEELEAVES/Delete/5
    public async Task<IActionResult> Delete(int? employeeleaveid)
    {
        if (employeeleaveid == null)
        {
            return NotFound();
        }

        var employeeleave = await _context.EmployeeLeaves
            .FirstOrDefaultAsync(m => m.EmployeeLeaveId == employeeleaveid);
        if (employeeleave == null)
        {
            return NotFound();
        }

        return View(employeeleave);
    }

    // POST: EMPLOYEELEAVES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? employeeleaveid)
    {
        var employeeleave = await _context.EmployeeLeaves.FindAsync(employeeleaveid);
        if (employeeleave != null)
        {
            _context.EmployeeLeaves.Remove(employeeleave);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EmployeeLeaveExists(int? employeeleaveid)
    {
        return _context.EmployeeLeaves.Any(e => e.EmployeeLeaveId == employeeleaveid);
    }
}
