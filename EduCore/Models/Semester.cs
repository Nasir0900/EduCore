using System.ComponentModel.DataAnnotations.Schema;
using EduCore.Models;

namespace EduCore.Models
{
    public class Semester
    {
        public int SemesterId { get; set; }

        public string SemesterName { get; set; } = string.Empty;

        public int SemesterNumber { get; set; }

        public int PartId { get; set; }

        [ForeignKey(nameof(PartId))]
        public Part? Part { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}