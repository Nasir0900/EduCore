using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCore.Enums;

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
        [Required]
       
        [StringLength(10)]
        [Display(Name = "Program Code")]
        public string ProgramCode { get; set; } = string.Empty;
        
        [StringLength(250)]


        public string? Description { get; set; }

        // Department
        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        // Program Information
        [Required]
        [Display(Name = "Program Type")]
        public ProgramType ProgramType { get; set; }

        [Required]
        [Display(Name = "Duration (Years)")]
        [Range(1, 10)]
        public int DurationYears { get; set; }

        [Required]
        [Display(Name = "Total Parts")]
        [Range(1, 10)]
        public int TotalParts { get; set; }

        [Required]
        [Display(Name = "Total Semesters")]
        [Range(1, 20)]
        public int TotalSemesters { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Property
        public ICollection<AcademicSession>? AcademicSessions { get; set; }
    }
}