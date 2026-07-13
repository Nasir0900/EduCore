using EduCore.Models;

namespace EduCore.Interfaces
{
    public interface ICourseService
    {
        Task<string> GenerateCourseCodeAsync(int departmentId, int semesterId);

        Task<bool> CourseCodeExistsAsync(string courseCode);

        Task<bool> CourseTitleExistsAsync(string title, int semesterId);

        Task<bool> ValidatePrerequisiteAsync(int semesterId, int? prerequisiteCourseId);

        Task<bool> ValidateCreditHoursAsync(int theoryHours, int practicalHours, int creditHours);

        Task<bool> ValidateMarksAsync(int totalMarks, int passingMarks);
    }
}