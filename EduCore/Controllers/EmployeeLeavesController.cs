
using EduCore.Data;
using EduCore.Enums;
using EduCore.Interfaces;
using EduCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace EduCore.Controllers;

public class EmployeeLeavesController(
    ApplicationDbContext context,
    ILeaveService leaveService) : Controller
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILeaveService _leaveService = leaveService;

    // Actions...


    //=========================================================
    // INDEX
    //=========================================================
    public async Task<IActionResult> Index()
    {
        var leaves = await _context.EmployeeLeaves
            .Include(l => l.Employee)
            .Include(l => l.LeaveType)
            .Include(l => l.ApprovedBy)
            .OrderByDescending(l => l.AppliedDate)
            .ToListAsync();

        return View(leaves);
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
    //=========================================================
    // APPROVE
    //=========================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var leave = await _context.EmployeeLeaves.FindAsync(id);

        if (leave == null)
            return NotFound();

        leave.Status = LeaveStatus.Approved;

        leave.ApprovalDate = DateTime.Now;

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Leave approved successfully.";

        return RedirectToAction(nameof(Index));
    }

    //=========================================================
    // REJECT
    //=========================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(
        int id,
        string remarks)
    {
        var leave = await _context.EmployeeLeaves.FindAsync(id);

        if (leave == null)
            return NotFound();

        leave.Status = LeaveStatus.Rejected;

        leave.ApprovalRemarks = remarks;

        leave.ApprovalDate = DateTime.Now;

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Leave rejected successfully.";

        return RedirectToAction(nameof(Index));
    }
    // GET: EMPLOYEELEAVES/Create
    public async Task<IActionResult> Create()
    {
        var leave = new EmployeeLeave
        {
            LeaveNumber = await _leaveService.GenerateLeaveNumberAsync(),
            FromDate = DateTime.Today,
            ToDate = DateTime.Today
        };

        ViewData["EmployeeId"] = new SelectList(
            _context.Employees.OrderBy(e => e.FirstName),
            "EmployeeId",
            "FullName");

        ViewData["LeaveTypeId"] = new SelectList(
            _context.LeaveTypes
                .Where(l => l.IsActive)
                .OrderBy(l => l.DisplayOrder),
            "LeaveTypeId",
            "LeaveTypeName");

        return View(leave);
    }
    //=========================================================
// GET: Check Leave Type
//=========================================================
[HttpGet]
public async Task<IActionResult> GetLeaveType(int leaveTypeId)
{
    var leaveType = await _context.LeaveTypes
        .Where(l => l.LeaveTypeId == leaveTypeId)
        .Select(l => new
        {
            requiresDocuments = l.RequiresDocuments
        })
        .FirstOrDefaultAsync();

    if (leaveType == null)
    {
        return Json(new
        {
            success = false
        });
    }

    return Json(new
    {
        success = true,
        leaveType.requiresDocuments
    });
}
    //=========================================================
    // CREATE (POST)
    //=========================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        EmployeeLeave leave,
        IFormFile? AttachmentFile)
    {
        //=====================================================
        // System Generated Values
        //=====================================================
        leave.LeaveNumber = await _leaveService.GenerateLeaveNumberAsync();

        leave.Status = LeaveStatus.Pending;

        leave.AppliedDate = DateTime.Now;

        //=====================================================
        // Calculate Total Days
        //=====================================================
        leave.TotalDays =
            (decimal)(leave.ToDate.Date - leave.FromDate.Date).TotalDays + 1;

        if (leave.IsHalfDay)
        {
            leave.TotalDays = 0.5M;
        }

        //=====================================================
        // Load Leave Type
        //=====================================================
        var leaveType = await _context.LeaveTypes
            .FirstOrDefaultAsync(l => l.LeaveTypeId == leave.LeaveTypeId);

        if (leaveType == null)
        {
            ModelState.AddModelError("", "Invalid leave type selected.");
        }
        else
        {
            //=============================================
            // Supporting Document Required
            //=============================================
            if (leaveType.RequiresDocuments && AttachmentFile == null)
            {
                ModelState.AddModelError(
                    "AttachmentFile",
                    "Supporting document is required for this leave type.");
            }

            //=============================================
            // Maximum Leave Days Validation
            //=============================================
            if (leave.TotalDays > leaveType.MaximumDays)
            {
                ModelState.AddModelError(
                    "TotalDays",
                    $"Maximum allowed days for {leaveType.LeaveTypeName} is {leaveType.MaximumDays}.");
            }

            //=============================================
            // Back-Date Validation
            //=============================================
            if (!leaveType.AllowBackDateApplication &&
                leave.FromDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(
                    "FromDate",
                    $"{leaveType.LeaveTypeName} cannot be applied for past dates.");
            }
        }

        //=====================================================
        // Date Validation
        //=====================================================
        if (leave.ToDate < leave.FromDate)
        {
            ModelState.AddModelError(
                "ToDate",
                "To Date cannot be earlier than From Date.");
        }

        //=====================================================
        // Half-Day Validation
        //=====================================================
        if (leave.IsHalfDay &&
            leave.FromDate.Date != leave.ToDate.Date)
        {
            ModelState.AddModelError(
                "IsHalfDay",
                "Half-day leave must be for a single day.");
        }

        //=====================================================
        // Save Leave
        //=====================================================
        if (ModelState.IsValid)
        {
            // Upload Supporting Document
            if (AttachmentFile != null && AttachmentFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "documents",
                    "leave");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(AttachmentFile.FileName);

                string filePath =
                    Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AttachmentFile.CopyToAsync(stream);
                }

                leave.AttachmentPath = fileName;
            }

            _context.EmployeeLeaves.Add(leave);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Leave application submitted successfully.";

            return RedirectToAction(nameof(Index));
        }

        //=====================================================
        // Reload DropDowns
        //=====================================================
        ViewData["EmployeeId"] = new SelectList(
            _context.Employees.OrderBy(e => e.FirstName),
            "EmployeeId",
            "FullName",
            leave.EmployeeId);

        ViewData["LeaveTypeId"] = new SelectList(
            _context.LeaveTypes
                .Where(l => l.IsActive)
                .OrderBy(l => l.DisplayOrder),
            "LeaveTypeId",
            "LeaveTypeName",
            leave.LeaveTypeId);

        return View(leave);
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
