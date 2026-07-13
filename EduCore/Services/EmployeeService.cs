using EduCore.Data;
using EduCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateEmployeeNumberAsync()
        {
            var lastEmployee = await _context.Employees
                .OrderByDescending(e => e.EmployeeId)
                .FirstOrDefaultAsync();

            if (lastEmployee == null)
                return "EMP-00001";

            string lastNumber = lastEmployee.EmployeeNumber.Replace("EMP-", "");

            int number = int.Parse(lastNumber);

            number++;

            return $"EMP-{number:00000}";
        }
    }
}