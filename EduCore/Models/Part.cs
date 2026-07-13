using EduCore.Models;

public class Part
{
    public int PartId { get; set; }

    public string PartName { get; set; } = string.Empty;

    public int PartNumber { get; set; }

    public int AcademicSessionId { get; set; }

    public AcademicSession? AcademicSession { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public ICollection<Semester> Semesters { get; set; } = new List<Semester>();
}