namespace TycoonCo.Domain
{
    public interface IWorkerRepository
    {
        Task<List<Worker>> GetList(ICollection<char> workerIds);
    }
}
