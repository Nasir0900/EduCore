namespace EduCore.Interfaces
{
    public interface ILeaveService
    {
        Task<string> GenerateLeaveNumberAsync();
    }
}