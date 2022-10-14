namespace TycoonCo.Application
{
    public static class ActivityMapper
    {
        public static void Map(Domain.Activity from, Activity to)
        {
            to.StartTime = from.StartTime;
            to.EndTime = from.EndTime;
            to.WorkerActivities = from.WorkerActivities.Select(wa => Map(wa)).ToArray();
            to.HasConflict = from.HasConflict;
            to.Type = (ActivityType)(int)from.Type;
            to.WorkersEndTime = from.WorkersEndTime;
        }

        public static Activity Map(Domain.Activity from)
        {
            var activity = new Activity();
            Map(from, activity);
            return activity;
        }

        public static WorkerActivity Map(Domain.WorkerActivity from)
        {
            return new WorkerActivity
            {
                HasConflict = from.HasConflict,
                WorkerId = from.WorkerId
            };
        }

        public static ActivityType Map(Domain.ActivityType from)
        {
            return from switch
            {
                Domain.ActivityType.BuildComponent => ActivityType.BuildComponent,
                Domain.ActivityType.BuildMachine => ActivityType.BuildMachine,
                _ => throw new NotSupportedException($"Unknown ActivityType: {from}"),
            };
        }
    }
}
