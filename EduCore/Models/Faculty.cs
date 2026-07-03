using System.ComponentModel.DataAnnotations;

namespace EduCore.Models
{
    public class Faculty
    {
        [Key]
        public int FacultyId { get; set; }

        [Required]
        [Display(Name = "Faculty Name")]
        [StringLength(100)]
        public string FacultyName { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Department>? Departments { get; set; }
    }
}