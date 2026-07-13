namespace EduCore.Interfaces
{
    public interface IEmployeeService
    {
        Task<string> GenerateEmployeeNumberAsync();
    }
}