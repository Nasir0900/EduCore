using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduCore.Models
{
    public class AcademicSession
    {
        [Key]
        public int AcademicSessionId { get; set; }

        [Required]
        [Display(Name = "Session Name")]
        [StringLength(20)]
        public string SessionName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Start Year")]
        public int StartYear { get; set; }

        [Required]
        [Display(Name = "End Year")]
        public int EndYear { get; set; }

        [Required]
        [Display(Name = "Academic Program")]
        public int AcademicProgramId { get; set; }

        [ForeignKey("AcademicProgramId")]
        public AcademicProgram? AcademicProgram { get; set; }

        [Display(Name = "Admissions Open")]
        public bool IsAdmissionOpen { get; set; } = true;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

      //  public ICollection<Part>? Parts { get; set; }
    }
}