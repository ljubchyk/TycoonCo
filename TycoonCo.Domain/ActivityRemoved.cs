namespace TycoonCo.Domain
{
    public class ActivityRemoved : DomainEvent
    {
        public ActivityRemoved(
            Guid activityId,
            DateTimeOffset startTime,
            DateTimeOffset endTime,
            ICollection<char> workerIds)
        {
            ActivityId = activityId;
            StartTime = startTime;
            EndTime = endTime;
            WorkerIds = workerIds;
        }

        public Guid ActivityId { get; }
        public DateTimeOffset StartTime { get; }
        public DateTimeOffset EndTime { get; }
        public ICollection<char> WorkerIds { get; }
    }
}
