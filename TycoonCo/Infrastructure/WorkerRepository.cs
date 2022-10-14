using Microsoft.EntityFrameworkCore;
using TycoonCo.Domain;

namespace TycoonCo.Infrastructure
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly Db db;

        public WorkerRepository(Db db)
        {
            this.db = db;
        }

        public Task<List<Worker>> GetList(ICollection<char> workerIds)
        {
            return db.Workers.Where(w => workerIds.Contains(w.Id)).ToListAsync();
        }
    }
}