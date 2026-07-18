using EduCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduCore.ViewModels.HumanResources;

public class EmployeeIndexViewModel
{
    // Data
    public List<Employee> Employees { get; set; } = [];

    // Search
    public string? SearchString { get; set; }

    // Filters
    public int? FacultyId { get; set; }

    public int? DepartmentId { get; set; }

    public int? DesignationId { get; set; }

    public int? EmploymentTypeId { get; set; }

    public bool? IsActive { get; set; }

    // Dropdowns
    public SelectList? Faculties { get; set; }

    public SelectList? Departments { get; set; }

    public SelectList? Designations { get; set; }

    public SelectList? EmploymentTypes { get; set; }

    // Statistics
    public int TotalEmployees => Employees.Count;

    public int ActiveEmployees =>
        Employees.Count(x => x.IsActive);

    public int InactiveEmployees =>
        Employees.Count(x => !x.IsActive);
}