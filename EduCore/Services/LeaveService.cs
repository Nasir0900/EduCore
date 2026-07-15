using EduCore.Data;
using EduCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ApplicationDbContext _context;

        public LeaveService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateLeaveNumberAsync()
        {
            int year = DateTime.Now.Year;

            var lastLeave = await _context.EmployeeLeaves
                .Where(l => l.LeaveNumber.StartsWith($"LV-{year}-"))
                .OrderByDescending(l => l.LeaveNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (lastLeave != null)
            {
                string lastPart = lastLeave.LeaveNumber.Substring(8);

                nextNumber = int.Parse(lastPart) + 1;
            }

            return $"LV-{year}-{nextNumber:D5}";
        }
    }
}