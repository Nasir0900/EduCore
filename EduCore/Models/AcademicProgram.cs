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
        [Display(Name = "Faculty")]
        public int FacultyId { get; set; }

        [ForeignKey("FacultyId")]
        public Faculty? Faculty { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}