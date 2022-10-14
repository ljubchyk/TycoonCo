namespace TycoonCo.Domain
{
    public interface IActivityRepository
    {
        Task<Activity> Get(Guid id);
        void Create(Activity activity);
        void Update(Activity activity);
        void Remove(Activity activity);
        void Update(ICollection<Activity> activities);
        Task<List<Activity>> GetConflictedActivities(
            DateTimeOffset startTime,
            DateTimeOffset endTime,
            ICollection<char> workerIds);
        Task<List<char>> GetOverlappedWorkerIds(
           DateTimeOffset startTime,
           DateTimeOffset endTime,
           ICollection<char> workerIds,
           Guid? outcludedActivityId = null);
        Task<List<char>> GetMostBusyWorkers(int limit, int nextDays);
    }
}
