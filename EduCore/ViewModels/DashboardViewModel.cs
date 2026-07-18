namespace EduCore.ViewModels
{
    public class DashboardViewModel
    {
        // Statistics
        public int EmployeeCount { get; set; }
        public int FacultyCount { get; set; }
        public int DepartmentCount { get; set; }
        public int CourseCount { get; set; }
        public int LeaveApplicationCount { get; set; }
        public int PendingLeaveCount { get; set; }

        // Welcome
        public string UserName { get; set; } = "";
        public string Greeting { get; set; } = "";

        // Dashboard Date
        public string Today { get; set; } = "";
    }
}