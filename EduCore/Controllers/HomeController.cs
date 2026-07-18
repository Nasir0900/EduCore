using EduCore.Data;
using EduCore.Enums;
using EduCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        string greeting;

        if (DateTime.Now.Hour < 12)
            greeting = "Good Morning";
        else if (DateTime.Now.Hour < 17)
            greeting = "Good Afternoon";
        else
            greeting = "Good Evening";

        DashboardViewModel vm = new()
        {
            EmployeeCount =
                await context.Employees.CountAsync(),

            FacultyCount =
                await context.Faculties.CountAsync(),

            DepartmentCount =
                await context.Departments.CountAsync(),

            CourseCount =
                await context.Courses.CountAsync(),

            LeaveApplicationCount =
                await context.EmployeeLeaves.CountAsync(),

            PendingLeaveCount =
                await context.EmployeeLeaves.CountAsync(
                    x => x.Status == LeaveStatus.Pending),

            Greeting = greeting,

            UserName = User.Identity?.Name ?? "Administrator",

            Today = DateTime.Now.ToString("dddd, dd MMMM yyyy")
        };

        return View(vm);
    }
}