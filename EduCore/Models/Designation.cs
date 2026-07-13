using System.ComponentModel.DataAnnotations;

namespace EduCore.Models
{
    public class Designation
    {
        [Key]
        public int DesignationId { get; set; }

        [Required]
        [Display(Name = "Designation")]
        [StringLength(100)]
        public string DesignationName { get; set; } = string.Empty;

        [Display(Name = "Short Name")]
        [StringLength(20)]
        public string? ShortName { get; set; }

        [Display(Name = "Teaching Staff")]
        public bool IsTeaching { get; set; } = true;

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

       // public ICollection<Teacher> Teachers { get; set; }
         //   = new List<Teacher>();
    }
}