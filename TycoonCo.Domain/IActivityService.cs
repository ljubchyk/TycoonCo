namespace TycoonCo.Domain
{
    public interface IActivityService
    {
        Task<Activity> Reschedule(Guid activityId, DateTimeOffset newStartTime, DateTimeOffset newEndTime);

        Task<Activity> Schedule(
            DateTimeOffset startTime,
            DateTimeOffset endTime,
            ActivityType type,
            ICollection<char> workerIds);

        Task AddConflictsWith(Guid activityId);

        Task RecalculateConflictsWith(
            Guid activityId, 
            DateTimeOffset oldStartTime, 
            DateTimeOffset oldEndTime, 
            ICollection<char> workerIds);

        Task Remove(Guid activityId);
    }
}