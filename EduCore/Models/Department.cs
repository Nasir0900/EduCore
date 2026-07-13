using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduCore.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Department Code")]
        [StringLength(10)]
        public string DepartmentCode { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Faculty")]
        public int FacultyId { get; set; }

        [ForeignKey(nameof(FacultyId))]
        public Faculty? Faculty { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<AcademicProgram> AcademicPrograms { get; set; }
            = new List<AcademicProgram>();

        public ICollection<Course> Courses { get; set; }
            = new List<Course>();

        // Future Modules
        // public ICollection<Teacher> Teachers { get; set; }
        //     = new List<Teacher>();
    }
}
