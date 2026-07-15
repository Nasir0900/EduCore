using EduCore.Data;
using EduCore.Interfaces;
using EduCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeService _employeeService;

        public EmployeesController(
            ApplicationDbContext context,
            IEmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
        }

        //=========================================================
        // INDEX
        //=========================================================
        public async Task<IActionResult> Index(
            string searchString,
            int? facultyId,
            int? departmentId,
            int? designationId,
            int? employmentTypeId,
            bool? isActive)
        {
            var employees = _context.Employees
                .Include(e => e.Faculty)
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.EmploymentType)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                employees = employees.Where(e =>
                    e.EmployeeNumber.Contains(searchString) ||
                    e.FirstName.Contains(searchString) ||
                    e.LastName.Contains(searchString) ||
                    e.CNIC.Contains(searchString));
            }

            // Faculty
            if (facultyId.HasValue)
            {
                employees = employees.Where(e =>
                    e.FacultyId == facultyId);
            }

            // Department
            if (departmentId.HasValue)
            {
                employees = employees.Where(e =>
                    e.DepartmentId == departmentId);
            }

            // Designation
            if (designationId.HasValue)
            {
                employees = employees.Where(e =>
                    e.DesignationId == designationId);
            }

            // Employment Type
            if (employmentTypeId.HasValue)
            {
                employees = employees.Where(e =>
                    e.EmploymentTypeId == employmentTypeId);
            }

            // Status
            if (isActive.HasValue)
            {
                employees = employees.Where(e =>
                    e.IsActive == isActive.Value);
            }

            // Dropdowns
            ViewBag.FacultyId = new SelectList(
                _context.Faculties.OrderBy(f => f.FacultyName),
                "FacultyId",
                "FacultyName",
                facultyId);

            ViewBag.DepartmentId = new SelectList(
                _context.Departments.OrderBy(d => d.DepartmentName),
                "DepartmentId",
                "DepartmentName",
                departmentId);

            ViewBag.DesignationId = new SelectList(
                _context.Designations.OrderBy(d => d.DisplayOrder),
                "DesignationId",
                "DesignationName",
                designationId);

            ViewBag.EmploymentTypeId = new SelectList(
                _context.EmploymentTypes.OrderBy(e => e.DisplayOrder),
                "EmploymentTypeId",
                "EmploymentTypeName",
                employmentTypeId);

            return View(await employees
                .OrderBy(e => e.EmployeeNumber)
                .ToListAsync());
        }
        //=========================================================
        // CREATE (GET)
        //=========================================================
        public async Task<IActionResult> Create()
        {
            var employee = new Employee
            {
                EmployeeNumber = await _employeeService.GenerateEmployeeNumberAsync()
            };

            LoadDropDowns();

            return View(employee);
        }
        //=========================================================
        // CREATE (POST)
        //=========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Employee employee,
            IFormFile? PhotoFile,
            IFormFile? SignatureFile)
        {
            employee.EmployeeNumber =
                await _employeeService.GenerateEmployeeNumberAsync();

            if (ModelState.IsValid)
            {
                // Upload Employee Photo
                if (PhotoFile != null && PhotoFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "employees");

                    string fileName =
                        Guid.NewGuid().ToString() +
                        Path.GetExtension(PhotoFile.FileName);

                    string filePath =
                        Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoFile.CopyToAsync(stream);
                    }

                    employee.PhotoPath = fileName;
                }
                // Upload Employee Signature
                if (SignatureFile != null && SignatureFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "signatures");

                    string fileName =
                        Guid.NewGuid().ToString() +
                        Path.GetExtension(SignatureFile.FileName);

                    string filePath =
                        Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await SignatureFile.CopyToAsync(stream);
                    }

                    employee.SignaturePath = fileName;
                }
                _context.Employees.Add(employee);

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Employee created successfully.";

                return RedirectToAction(nameof(Index));
            }

            LoadDropDowns(
                employee.FacultyId,
                employee.DepartmentId,
                employee.DesignationId,
                employee.EmploymentTypeId);

            return View(employee);
        }

        //=========================================================
        // EDIT (GET)
        //=========================================================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            LoadDropDowns(
                employee.FacultyId,
                employee.DepartmentId,
                employee.DesignationId,
                employee.EmploymentTypeId);

            return View(employee);
        }

        //=========================================================
        // EDIT (POST)
        //=========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
             int id,
             Employee employee,
             IFormFile? PhotoFile,
             IFormFile? SignatureFile)
        {
            if (id != employee.EmployeeId)
                return NotFound();

            // Never allow Employee Number to change
            var existingEmployee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
            
            if (existingEmployee == null)
                return NotFound();
            // Keep existing Employee Number
            employee.EmployeeNumber = existingEmployee.EmployeeNumber;

            // Keep existing photo
            employee.PhotoPath = existingEmployee.PhotoPath;
            // Keep Existing signature
            employee.SignaturePath = existingEmployee.SignaturePath;
            

            if (ModelState.IsValid)
            {
                try
                {
                    // Upload new photo
                    if (PhotoFile != null && PhotoFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            "images",
                            "employees");

                        string fileName = Guid.NewGuid().ToString()
                            + Path.GetExtension(PhotoFile.FileName);

                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await PhotoFile.CopyToAsync(stream);
                        }

                        // Delete old photo (if there is one)
                        if (!string.IsNullOrEmpty(existingEmployee.PhotoPath))
                        {
                            string oldFile = Path.Combine(
                                uploadsFolder,
                                existingEmployee.PhotoPath);

                            if (System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }
                        }

                        employee.PhotoPath = fileName;
                    }

                    // Upload Employee Signature
                    if (SignatureFile != null && SignatureFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            "images",
                            "signatures");

                        string fileName = Guid.NewGuid().ToString()
                            + Path.GetExtension(SignatureFile.FileName);

                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await SignatureFile.CopyToAsync(stream);
                        }

                        // Delete old signature
                        if (!string.IsNullOrEmpty(existingEmployee.SignaturePath))
                        {
                            string oldFile = Path.Combine(
                                uploadsFolder,
                                existingEmployee.SignaturePath!);

                            if (System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }
                        }

                        employee.SignaturePath = fileName;
                    }

                    _context.Update(employee);

                    await _context.SaveChangesAsync();

                    TempData["Success"] =
                        "Employee updated successfully.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Employees.Any(e => e.EmployeeId == employee.EmployeeId))
                        return NotFound();

                    throw;
                }
            }

            LoadDropDowns(
                employee.FacultyId,
                employee.DepartmentId,
                employee.DesignationId,
                employee.EmploymentTypeId);

            return View(employee);
        }

        //=========================================================
        // DETAILS
        //=========================================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Faculty)
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.EmploymentType)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        //=========================================================
        // DELETE (GET)
        //=========================================================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }
        //=========================================================
        // DELETE (POST)
        //=========================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);

                await _context.SaveChangesAsync();

                TempData["Success"] = "Employee deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
        //=========================================================
        // LOAD DROPDOWNS
        //=========================================================
        private void LoadDropDowns(
            int? facultyId = null,
            int? departmentId = null,
            int? designationId = null,
            int? employmentTypeId = null)
        {
            ViewData["FacultyId"] = new SelectList(
                _context.Faculties.OrderBy(f => f.FacultyName),
                "FacultyId",
                "FacultyName",
                facultyId);

            ViewData["DepartmentId"] = new SelectList(
                _context.Departments.OrderBy(d => d.DepartmentName),
                "DepartmentId",
                "DepartmentName",
                departmentId);

            ViewData["DesignationId"] = new SelectList(
                _context.Designations.OrderBy(d => d.DisplayOrder),
                "DesignationId",
                "DesignationName",
                designationId);

            ViewData["EmploymentTypeId"] = new SelectList(
                _context.EmploymentTypes.OrderBy(e => e.DisplayOrder),
                "EmploymentTypeId",
                "EmploymentTypeName",
                employmentTypeId);
        }

        //=========================================================
        // GET DEPARTMENTS BY FACULTY (AJAX)
        //=========================================================
        [HttpGet]
        public async Task<IActionResult> GetDepartments(int facultyId)
        {
            var departments = await _context.Departments
                .Where(d => d.FacultyId == facultyId)
                .OrderBy(d => d.DepartmentName)
                .Select(d => new
                {
                    departmentId = d.DepartmentId,
                    departmentName = d.DepartmentName
                })
                .ToListAsync();

            return Json(departments);
        }

    }

}