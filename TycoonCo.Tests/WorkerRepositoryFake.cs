using TycoonCo.Domain;

namespace TycoonCo.Tests
{
    public class WorkerRepositoryFake : IWorkerRepository
    {
        private List<Worker> workers;

        public WorkerRepositoryFake()
        {
            workers = new List<Worker>
            {
                new Worker('A'),
                new Worker('B'),
                new Worker('C'),
                new Worker('D'),
                new Worker('E')
            };
        }

        public Task<List<Worker>> GetList(ICollection<char> workerIds)
        {
            return Task.FromResult(workers);
        }
    }
}