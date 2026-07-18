using EduCore.Data;
using EduCore.Enums;
using EduCore.ViewModels.HumanResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Controllers;

public class EmployeeWorkspaceController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var employees = await context.Employees
            .Include(e => e.Designation)
            .OrderByDescending(e => e.EmployeeId)
            .Take(5)
            .ToListAsync();
        var vm = new EmployeeWorkspaceViewModel
        {
            EmployeeCount = await context.Employees.CountAsync(),

            ActiveEmployeeCount = await context.Employees
                .CountAsync(e => e.IsActive),

            // Temporary until we add Employee Categories
            TeachingStaffCount = await context.Employees.CountAsync(),

            NonTeachingStaffCount = 0,

            RecentEmployees = employees,

            PendingLeaveCount = await context.EmployeeLeaves
                .CountAsync(x => x.Status == LeaveStatus.Pending)
        };

        return View(vm);
    }
}