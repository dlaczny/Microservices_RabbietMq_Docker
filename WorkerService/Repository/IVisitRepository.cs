
namespace WorkerService.Repository
{
    public interface IVisitRepository
    {
        Task ProcessFileAsync(string filePath);
    }
}