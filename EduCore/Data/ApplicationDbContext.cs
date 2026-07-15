using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EduCore.Models;

namespace EduCore.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext(options)
    {
        public DbSet<Department> Departments { get; set; }

        public DbSet<Faculty> Faculties { get; set; }

        public DbSet<AcademicProgram> AcademicPrograms { get; set; }

        public DbSet<AcademicSession> AcademicSessions { get; set; }

        public DbSet<Part> Parts { get; set; }

        public DbSet<Semester> Semesters { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Designation> Designations { get; set; }

        public DbSet<EmploymentType> EmploymentTypes { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<LeaveType> LeaveTypes { get; set; }

        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Semester)
                .WithMany(s => s.Courses)
                .HasForeignKey(c => c.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.PrerequisiteCourse)
                .WithMany(c => c.DependentCourses)
                .HasForeignKey(c => c.PrerequisiteCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            //=========================================================
            // Employee Relationships
            //=========================================================

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Faculty)
                .WithMany()
                .HasForeignKey(e => e.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Designation)
                .WithMany()
                .HasForeignKey(e => e.DesignationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.EmploymentType)
                .WithMany()
                .HasForeignKey(e => e.EmploymentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                .HasIndex(d => d.DepartmentCode)
                .IsUnique();
            //=====================================================
            // Employee Leave Relationships
            //=====================================================

            modelBuilder.Entity<EmployeeLeave>()
                .HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeLeave>()
                .HasOne(e => e.ApprovedBy)
                .WithMany()
                .HasForeignKey(e => e.ApprovedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeLeave>()
                .HasOne(e => e.LeaveType)
                .WithMany(l => l.EmployeeLeaves)
                .HasForeignKey(e => e.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveType>()
                .Property(l => l.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<LeaveType>().HasData(

    new LeaveType
    {
        LeaveTypeId = 1,
        LeaveCode = "CL",
        LeaveTypeName = "Casual Leave",
        MaximumDays = 10,
        IsPaidLeave = true,
        RequiresApproval = true,
        RequiresDocuments = false,
        AllowCarryForward = false,
        AllowBackDateApplication = false,
        AvailableAfterMonths = 0,
        IsYearlyLimit = true,
        DisplayOrder = 1,
        IsActive = true
    },

    new LeaveType
    {
        LeaveTypeId = 2,
        LeaveCode = "AL",
        LeaveTypeName = "Annual Leave",
        MaximumDays = 30,
        IsPaidLeave = true,
        RequiresApproval = true,
        RequiresDocuments = false,
        AllowCarryForward = true,
        AllowBackDateApplication = false,
        AvailableAfterMonths = 12,
        IsYearlyLimit = true,
        DisplayOrder = 2,
        IsActive = true
    },

    new LeaveType
    {
        LeaveTypeId = 3,
        LeaveCode = "ML",
        LeaveTypeName = "Medical Leave",
        MaximumDays = 20,
        IsPaidLeave = true,
        RequiresApproval = true,
        RequiresDocuments = true,
        AllowCarryForward = false,
        AllowBackDateApplication = true,
        AvailableAfterMonths = 0,
        IsYearlyLimit = true,
        DisplayOrder = 3,
        IsActive = true
    },

    new LeaveType
    {
        LeaveTypeId = 4,
        LeaveCode = "SL",
        LeaveTypeName = "Study Leave",
        MaximumDays = 365,
        IsPaidLeave = true,
        RequiresApproval = true,
        RequiresDocuments = true,
        AllowCarryForward = false,
        AllowBackDateApplication = false,
        AvailableAfterMonths = 24,
        IsYearlyLimit = false,
        DisplayOrder = 4,
        IsActive = true
    },

    new LeaveType
    {
        LeaveTypeId = 5,
        LeaveCode = "MAT",
        LeaveTypeName = "Maternity Leave",
        MaximumDays = 180,
        IsPaidLeave = true,
        RequiresApproval = true,
        RequiresDocuments = true,
        AllowCarryForward = false,
        AllowBackDateApplication = false,
        AvailableAfterMonths = 0,
        IsYearlyLimit = false,
        DisplayOrder = 5,
        IsActive = true
    },

    new LeaveType
    {
        LeaveTypeId = 6,
        LeaveCode = "PAT",
        LeaveTypeName = "Paternity Leave",
        MaximumDays = 15,
        IsPaidLeave = true,
        RequiresApproval = true,
        RequiresDocuments = false,
        AllowCarryForward = false,
        AllowBackDateApplication = false,
        AvailableAfterMonths = 0,
        IsYearlyLimit = false,
        DisplayOrder = 6,
        IsActive = true
    },

    new LeaveType
    {
        LeaveTypeId = 7,
        LeaveCode = "EOL",
        LeaveTypeName = "Extraordinary Leave",
        MaximumDays = 365,
        IsPaidLeave = false,
        RequiresApproval = true,
        RequiresDocuments = true,
        AllowCarryForward = false,
        AllowBackDateApplication = false,
        AvailableAfterMonths = 0,
        IsYearlyLimit = false,
        DisplayOrder = 7,
        IsActive = true
    }

);

        }
    }
}