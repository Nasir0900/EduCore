using EduCore.Data;
using EduCore.Interfaces;
using EduCore.Models;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Generate Course Code
        public async Task<string> GenerateCourseCodeAsync(int departmentId, int semesterId)
        {
            // Get Department
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            if (department == null)
                throw new Exception("Department not found.");

            // Get Semester with Part
            var semester = await _context.Semesters
                .Include(s => s.Part)
                .FirstOrDefaultAsync(s => s.SemesterId == semesterId);

            if (semester == null)
                throw new Exception("Semester not found.");

            if (semester.Part == null)
                throw new Exception("Part not found.");

            string departmentCode = department.DepartmentCode.ToUpper();

            int partNumber = semester.Part.PartNumber;
            int semesterNumber = semester.SemesterNumber;

            int seriesBase = GetSeriesBase(partNumber, semesterNumber);

            // Find existing courses in the same department
            // having the same series
            var existingNumbers = await _context.Courses
                .Where(c => c.DepartmentId == departmentId)
                .Select(c => c.CourseCode)
                .ToListAsync();

            int maxNumber = seriesBase;

            foreach (var code in existingNumbers)
            {
                if (string.IsNullOrWhiteSpace(code))
                    continue;

                if (!code.StartsWith(departmentCode + "-"))
                    continue;

                var numberPart = code.Substring(code.IndexOf('-') + 1);

                if (!int.TryParse(numberPart, out int number))
                    continue;

                if (number >= seriesBase && number < seriesBase + 100)
                {
                    if (number > maxNumber)
                        maxNumber = number;
                }
            }

            int nextNumber = maxNumber + 1;

            return $"{departmentCode}-{nextNumber}";
        }
        private int GetSeriesBase(int partNumber, int semesterNumber)
        {
            switch (partNumber)
            {
                case 1:
                    return semesterNumber == 1 ? 100 : 200;

                case 2:
                    return semesterNumber == 3 ? 300 : 400;

                case 3:
                    return 500;

                case 4:
                    return 600;

                case 5:
                    return 700;

                case 6:
                    return 800;

                default:
                    throw new Exception("Invalid Part Number.");
            }
        }
        // Check Duplicate Course Code
        public async Task<bool> CourseCodeExistsAsync(string courseCode)
        {
            return await _context.Courses
                .AnyAsync(c => c.CourseCode == courseCode);
        }

        // Check Duplicate Course Title
        public async Task<bool> CourseTitleExistsAsync(string title, int semesterId)
        {
            return await _context.Courses
                .AnyAsync(c =>
                    c.CourseTitle == title &&
                    c.SemesterId == semesterId);
        }

        // Validate Prerequisite
        public async Task<bool> ValidatePrerequisiteAsync(int semesterId, int? prerequisiteCourseId)
        {
            if (prerequisiteCourseId == null)
                return true;

            var prerequisite = await _context.Courses
                .Include(c => c.Semester)
                .FirstOrDefaultAsync(c => c.CourseId == prerequisiteCourseId);

            if (prerequisite?.Semester == null)
                return false;

            int currentSemester = await _context.Semesters
                .Where(s => s.SemesterId == semesterId)
                .Select(s => s.SemesterNumber)
                .FirstAsync();

            return prerequisite.Semester.SemesterNumber < currentSemester;
        }

        // Validate Credit Hours
        public Task<bool> ValidateCreditHoursAsync(
            int theoryHours,
            int practicalHours,
            int creditHours)
        {
            return Task.FromResult(theoryHours + practicalHours == creditHours);
        }

        // Validate Marks
        public Task<bool> ValidateMarksAsync(
            int totalMarks,
            int passingMarks)
        {
            return Task.FromResult(passingMarks < totalMarks);
        }
    }
}