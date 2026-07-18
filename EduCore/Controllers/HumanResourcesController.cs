using EduCore.Data;
using EduCore.Enums;
using EduCore.ViewModels.HumanResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Controllers;

public class HumanResourcesController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IActionResult> Index()
    {
        var vm = new HumanResourcesDashboardViewModel
        {
            EmployeeCount = await _context.Employees.CountAsync(),

            PendingLeaveCount = await _context.EmployeeLeaves
                .CountAsync(x => x.Status == LeaveStatus.Pending),

            // Temporary until Employee Category is implemented
            TeachingStaffCount = await _context.Employees.CountAsync(),

            NonTeachingStaffCount = 0
        };

        return View(vm);
    }
}