using EduCore.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduCore.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        // ==========================
        // Basic Information
        // ==========================

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

        [Required]
        [Display(Name = "Course Code")]
        [StringLength(20)]
        public string CourseCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Course Title")]
        [StringLength(200)]
        public string CourseTitle { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [StringLength(500)]
        public string? Description { get; set; }

        // ==========================
        // Academic Structure
        // ==========================


        [Required]
        [Display(Name = "Semester")]
        public int SemesterId { get; set; }

        [ForeignKey(nameof(SemesterId))]
        public Semester? Semester { get; set; }

        // ==========================
        // Course Classification
        // ==========================

        [Required]
        [Display(Name = "Course Type")]
        public CourseType CourseType { get; set; }

        [Required]
        [Display(Name = "Course Category")]
        public CourseCategory CourseCategory { get; set; }

        [Display(Name = "Compulsory")]
        public bool IsCompulsory { get; set; } = true;

        // ==========================
        // Credit Structure
        // ==========================

        [Required]
        [Display(Name = "Credit Hours")]
        [Range(1, 6)]
        public int CreditHours { get; set; }

        [Required]
        [Display(Name = "Theory Hours")]
        [Range(0, 6)]
        public int TheoryHours { get; set; }

        [Required]
        [Display(Name = "Practical Hours")]
        [Range(0, 6)]
        public int PracticalHours { get; set; }

        // ==========================
        // Examination
        // ==========================

        [Display(Name = "Total Marks")]
        [Range(1, 1000)]
        public int TotalMarks { get; set; } = 100;

        [Display(Name = "Passing Marks")]
        [Range(1, 1000)]
        public int PassingMarks { get; set; } = 50;

        // ==========================
        // Course Ordering
        // ==========================

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        // ==========================
        // Enrollment
        // ==========================

        [Display(Name = "Maximum Enrollment")]
        public int? MaximumEnrollment { get; set; }

        // ==========================
        // Prerequisite
        // ==========================

        [Display(Name = "Prerequisite Course")]
        public int? PrerequisiteCourseId { get; set; }

        [ForeignKey(nameof(PrerequisiteCourseId))]
        public Course? PrerequisiteCourse { get; set; }

        public ICollection<Course> DependentCourses { get; set; } = new List<Course>();

        // ==========================
        // Status
        // ==========================

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}