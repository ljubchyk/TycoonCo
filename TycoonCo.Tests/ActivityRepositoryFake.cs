using TycoonCo.Domain;

namespace TycoonCo.Tests
{
    //fake repository must store copies
    public class ActivityRepositoryFake : IActivityRepository
    {
        private readonly List<Activity> activities;

        public ActivityRepositoryFake()
        {
            activities = new List<Activity>();
        }

        public void Create(Activity activity)
        {
            activities.Add(activity);
        }

        public Task<Activity?> Get(Guid id)
        {
            return Task.FromResult(activities.Find(a => a.Id == id));
        }

        public Task<List<Activity>> GetConflictedActivities(
            DateTimeOffset startTime, 
            DateTimeOffset endTime, 
            ICollection<char> workerIds)
        {
            var conflictedActivities = activities
                .Where(a => a.StartTime <= endTime && a.WorkersEndTime >= startTime)
                .ToList();
            return Task.FromResult(conflictedActivities);
        }

        public Task<List<char>> GetMostBusyWorkers(int limit, int nextDays)
        {
            throw new NotImplementedException();
        }

        public Task<List<char>> GetOverlappedWorkerIds(
            DateTimeOffset startTime,
            DateTimeOffset endTime,
            ICollection<char> workerIds,
            Guid? outcludedActivityId = null)
        {
            var result = activities.
                Where(
                a =>
                (outcludedActivityId is null || outcludedActivityId != a.Id) &&
                a.StartTime <= endTime &&
                a.WorkersEndTime >= startTime).
                SelectMany(a => a.WorkerActivities.Select(wa => wa.WorkerId)).
                Distinct().
                ToList();

            return Task.FromResult(result);
        }

        public void Remove(Activity activity)
        {
            throw new NotImplementedException();
        }

        public void Update(Activity activity)
        {
            //fake repository must store copies
            //than this method will have logic
        }

        public void Update(ICollection<Activity> activities)
        {
            throw new NotImplementedException();
        }
    }
}