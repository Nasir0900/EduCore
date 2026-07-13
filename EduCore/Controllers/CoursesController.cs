using EduCore.Data;
using EduCore.Interfaces;
using EduCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseService _courseService;

        public CoursesController(
            ApplicationDbContext context,
            ICourseService courseService)
        {
            _context = context;
            _courseService = courseService;
        }

        //=========================================================
        // INDEX
        //=========================================================
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Semester)
                .Include(c => c.PrerequisiteCourse)
                .OrderBy(c => c.Department!.DepartmentName)
                .ThenBy(c => c.Semester!.SemesterNumber)
                .ThenBy(c => c.DisplayOrder)
                .ToListAsync();

            return View(courses);

        }
        //=========================================================
        // CREATE (GET)
        //=========================================================
        public IActionResult Create()
        {
            ViewData["FacultyId"] = new SelectList(
            _context.Faculties
            .OrderBy(f => f.FacultyName),
            "FacultyId",
            "FacultyName");
            LoadDropDowns();

            return View();
        }

        //=========================================================
        // CREATE (POST)
        //=========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            // Generate Course Code
            course.CourseCode =
     await _courseService.GenerateCourseCodeAsync(
         course.DepartmentId,
         course.SemesterId);

            // Duplicate Course Code
            if (await _courseService.CourseCodeExistsAsync(course.CourseCode))
            {
                ModelState.AddModelError(
                    "CourseCode",
                    "Course Code already exists.");
            }

            // Duplicate Course Title
            if (await _courseService.CourseTitleExistsAsync(
                course.CourseTitle,
                course.SemesterId))
            {
                ModelState.AddModelError(
                    "CourseTitle",
                    "A course with this title already exists in the selected semester.");
            }

            course.CourseCode = await _courseService.GenerateCourseCodeAsync(
            course.DepartmentId,
            course.SemesterId);

            // Credit Hours Validation
            if (!await _courseService.ValidateCreditHoursAsync(
                course.TheoryHours,
                course.PracticalHours,
                course.CreditHours))
            {
                ModelState.AddModelError(
                    "CreditHours",
                    "Theory Hours + Practical Hours must equal Credit Hours.");
            }

            // Marks Validation
            if (!await _courseService.ValidateMarksAsync(
                course.TotalMarks,
                course.PassingMarks))
            {
                ModelState.AddModelError(
                    "PassingMarks",
                    "Passing Marks must be less than Total Marks.");
            }

            // Prerequisite Validation
            if (!await _courseService.ValidatePrerequisiteAsync(
                course.SemesterId,
                course.PrerequisiteCourseId))
            {
                ModelState.AddModelError(
                    "PrerequisiteCourseId",
                    "Prerequisite must belong to an earlier semester.");
            }

            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);

                await _context.SaveChangesAsync();

                TempData["Success"] = "Course created successfully.";

                return RedirectToAction(nameof(Index));
            }

            LoadDropDowns(
              course.DepartmentId,
              course.SemesterId,
              course.PrerequisiteCourseId);

            return View(course);
        }

        //=========================================================
        // AJAX - GENERATE COURSE CODE
        //=========================================================

        [HttpGet]
        public async Task<IActionResult> GetDepartments(int facultyId)
        {
            var departments = await _context.Departments
                .Where(d => d.FacultyId == facultyId)
                .OrderBy(d => d.DepartmentName)
                .Select(d => new
                {
                    d.DepartmentId,
                    d.DepartmentName
                })
                .ToListAsync();

            return Json(departments);
        }
        [HttpGet]
        public async Task<IActionResult> GetAcademicSessions(int academicProgramId)
        {
            var sessions = await _context.AcademicSessions
                .Where(s => s.AcademicProgramId == academicProgramId)
                .OrderByDescending(s => s.SessionName)
                .Select(s => new
                {
                    s.AcademicSessionId,
                    s.SessionName
                })
                .ToListAsync();

            return Json(sessions);
        }
        [HttpGet]
        public async Task<IActionResult> GetSemesters(int academicSessionId)
        {
            var semesters = await _context.Semesters
                .Include(s => s.Part)
                .Where(s => s.Part!.AcademicSessionId == academicSessionId)
                .OrderBy(s => s.SemesterNumber)
                .Select(s => new
                {
                    s.SemesterId,
                    s.SemesterName
                })
                .ToListAsync();

            return Json(semesters);
        }
        [HttpGet]
        public async Task<IActionResult> GetPrograms(int departmentId)
        {
            var programs = await _context.AcademicPrograms
                .Where(p => p.DepartmentId == departmentId)
                .OrderBy(p => p.ProgramName)
                .Select(p => new
                {
                    p.AcademicProgramId,
                    p.ProgramName
                })
                .ToListAsync();

            return Json(programs);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateCourseCode(int departmentId, int semesterId)
        {
            try
            {
                string code = await _courseService.GenerateCourseCodeAsync(
                    departmentId,
                    semesterId);

                return Json(new
                {
                    success = true,
                    courseCode = code
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }



        //=========================================================
        // DETAILS
        //=========================================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Semester)
                .Include(c => c.PrerequisiteCourse)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        //=========================================================
        // EDIT (GET)
        //=========================================================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound();

            LoadDropDowns(
                 course.DepartmentId,
                 course.SemesterId,
                 course.PrerequisiteCourseId);

            return View(course);
        }

        //=========================================================
        // DELETE (GET)
        //=========================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Course deleted successfully.";

            return RedirectToAction(nameof(Index));
        }


        //=========================================================
        // LOAD DROPDOWNS
        //=========================================================
        private void LoadDropDowns(
     int? departmentId = null,
     int? semesterId = null,
     int? prerequisiteId = null)
        {
            ViewData["DepartmentId"] = new SelectList(
                _context.Departments
                    .OrderBy(d => d.DepartmentName),
                "DepartmentId",
                "DepartmentName",
                departmentId);

            ViewData["SemesterId"] = new SelectList(
                _context.Semesters
                    .OrderBy(s => s.SemesterNumber),
                "SemesterId",
                "SemesterName",
                semesterId);

            ViewData["PrerequisiteCourseId"] = new SelectList(
                _context.Courses
                    .OrderBy(c => c.CourseTitle),
                "CourseId",
                "CourseTitle",
                prerequisiteId);
        }
    }
}