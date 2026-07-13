using System.ComponentModel.DataAnnotations;

namespace EduCore.Models
{
    public class EmploymentType
    {
        [Key]
        public int EmploymentTypeId { get; set; }

        [Required]
        [Display(Name = "Employment Type")]
        [StringLength(100)]
        public string EmploymentTypeName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [StringLength(250)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}