namespace TycoonCo.Application
{
    public class Activity
    {
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public DateTimeOffset WorkersEndTime { get; internal set; }
        public ActivityType Type { get; set; }
        public bool HasConflict { get; internal set; }
        public IEnumerable<WorkerActivity> WorkerActivities { get; set; }
    }
}
