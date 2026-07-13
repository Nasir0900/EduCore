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
        }
    }
}