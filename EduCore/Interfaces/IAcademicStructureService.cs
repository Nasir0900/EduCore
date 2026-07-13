using EduCore.Models;

namespace EduCore.Interfaces
{
    public interface IAcademicStructureService
    {
        Task GenerateAcademicStructureAsync(int academicSessionId);

        Task<bool> StructureExistsAsync(int academicSessionId);
    }
}