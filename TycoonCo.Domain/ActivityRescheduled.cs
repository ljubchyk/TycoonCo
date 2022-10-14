namespace TycoonCo.Domain
{
    public class ActivityRescheduled : DomainEvent
    {
        public ActivityRescheduled(
            Guid activityId, 
            DateTimeOffset newStartTime,
            DateTimeOffset newEndTime, 
            DateTimeOffset oldStartTime, 
            DateTimeOffset oldEndTime,
            ICollection<char> workerIds)
        {
            ActivityId = activityId;
            NewStartTime = newStartTime;
            NewEndTime = newEndTime;
            OldStartTime = oldStartTime;
            OldEndTime = oldEndTime;
            WorkerIds = workerIds;
        }

        public Guid ActivityId { get; }
        public DateTimeOffset NewStartTime { get; }
        public DateTimeOffset NewEndTime { get; }
        public DateTimeOffset OldStartTime { get; }
        public DateTimeOffset OldEndTime { get; }
        public ICollection<char> WorkerIds { get; }
    }
}
