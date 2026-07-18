using EduCore.Models;
using EduCore.Enums;

namespace EduCore.ViewModels.HumanResources;

public class EmployeeWorkspaceViewModel
{
    public int EmployeeCount { get; set; }

    public int ActiveEmployeeCount { get; set; }

    public int TeachingStaffCount { get; set; }

    public int NonTeachingStaffCount { get; set; }

    public List<Employee> RecentEmployees { get; set; } = [];

    public int PendingLeaveCount { get; set; }
}