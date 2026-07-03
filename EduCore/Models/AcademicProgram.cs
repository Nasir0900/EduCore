using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduCore.Models
{
    public class AcademicProgram
    {
        [Key]
        public int AcademicProgramId { get; set; }

        [Required]
        [Display(Name = "Program Name")]
        [StringLength(100)]
        public string ProgramName { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Future relationship
      //  public ICollection<Semester>? Semesters { get; set; }
    }
}