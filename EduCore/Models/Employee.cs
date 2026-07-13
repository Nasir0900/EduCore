using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCore.Enums;

namespace EduCore.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        //=================================================
        // Basic Information
        //=================================================

        [Required]
        [Display(Name = "Employee Number")]
        [StringLength(20)]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Father Name")]
        [StringLength(100)]
        public string? FatherName { get; set; }

        [Required]
        [Display(Name = "CNIC")]
        [StringLength(15)]
        public string CNIC { get; set; } = string.Empty;

        [Required]
        public Gender Gender { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Marital Status")]
        public MaritalStatus MaritalStatus { get; set; }

        [Display(Name = "Blood Group")]
        public BloodGroup BloodGroup { get; set; }

        //=================================================
        // Contact Information
        //=================================================

        [Display(Name = "Mobile Number")]
        [StringLength(20)]
        public string? MobileNumber { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Address")]
        [StringLength(300)]
        public string? Address { get; set; }

        //=================================================
        // Employment Information
        //=================================================

        [Required]
        public int FacultyId { get; set; }

        [ForeignKey(nameof(FacultyId))]
        public Faculty? Faculty { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

        [Required]
        public int DesignationId { get; set; }

        [ForeignKey(nameof(DesignationId))]
        public Designation? Designation { get; set; }

        [Required]
        public int EmploymentTypeId { get; set; }

        [ForeignKey(nameof(EmploymentTypeId))]
        public EmploymentType? EmploymentType { get; set; }

        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; } = DateTime.Today;

        [Display(Name = "Employment Status")]
        public EmploymentStatus EmploymentStatus { get; set; } = EmploymentStatus.Active;

        //=================================================
        // Academic Information
        //=================================================

        [Display(Name = "Highest Qualification")]
        [StringLength(150)]
        public string? Qualification { get; set; }

        [Display(Name = "Specialization")]
        [StringLength(150)]
        public string? Specialization { get; set; }

        [Display(Name = "Experience (Years)")]
        public int ExperienceYears { get; set; }

        //=================================================
        // System
        //=================================================

        [Display(Name = "Photo")]
        public string? PhotoPath { get; set; }

        [Display(Name = "Signature")]
        public string? SignaturePath { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        //=================================================
        // Computed Property
        //=================================================

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}