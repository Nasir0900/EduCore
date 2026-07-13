using EduCore.Data;
using EduCore.Interfaces;
using EduCore.Models;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Services
{
    public class AcademicStructureService : IAcademicStructureService
    {
        private readonly ApplicationDbContext _context;

        public AcademicStructureService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> StructureExistsAsync(int academicSessionId)
        {
            return await _context.Parts
                .AnyAsync(p => p.AcademicSessionId == academicSessionId);
        }

        public async Task GenerateAcademicStructureAsync(int academicSessionId)
        {
            // Prevent duplicate generation
            if (await StructureExistsAsync(academicSessionId))
            {
                throw new Exception("Academic Structure has already been generated for this Academic Session.");
            }

            // Load Academic Session and Program
            var session = await _context.AcademicSessions
                .Include(s => s.AcademicProgram)
                .FirstOrDefaultAsync(s => s.AcademicSessionId == academicSessionId);

            if (session == null)
                throw new Exception("Academic Session not found.");

            if (session.AcademicProgram == null)
                throw new Exception("Academic Program not found.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                int semesterCounter = 1;

                for (int partNumber = 1; partNumber <= session.AcademicProgram.TotalParts; partNumber++)
                {
                    var part = new Part
                    {
                        PartName = $"Part {ToRoman(partNumber)}",
                        PartNumber = partNumber,
                        AcademicSessionId = session.AcademicSessionId,
                        CreatedDate = DateTime.Now
                    };

                    _context.Parts.Add(part);

                    // Save once to get PartId
                    await _context.SaveChangesAsync();

                    // Generate two semesters for each part
                    for (int i = 0;
                         i < 2 && semesterCounter <= session.AcademicProgram.TotalSemesters;
                         i++)
                    {
                        var semester = new Semester
                        {
                            SemesterName = $"Semester {ToRoman(semesterCounter)}",
                            SemesterNumber = semesterCounter,
                            PartId = part.PartId,
                            CreatedDate = DateTime.Now
                        };

                        _context.Semesters.Add(semester);

                        semesterCounter++;
                    }

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static string ToRoman(int number)
        {
            return number switch
            {
                1 => "I",
                2 => "II",
                3 => "III",
                4 => "IV",
                5 => "V",
                6 => "VI",
                7 => "VII",
                8 => "VIII",
                9 => "IX",
                10 => "X",
                _ => number.ToString()
            };
        }
    }
}