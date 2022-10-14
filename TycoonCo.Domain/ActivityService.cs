using System.Diagnostics;
using System.Reflection;

namespace TycoonCo.Domain
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository activityRepository;

        public ActivityService(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public async Task AddConflictsWith(Guid activityId)
        {
            var activity = await activityRepository.Get(activityId);
            if (activity is null)
            {
                return;
            }

            var workerIds = activity.GetWorkerIds();
            var conflictedActivities =
                await activityRepository
                .GetConflictedActivities(
                    activity.StartTime,
                    activity.EndTime,
                    workerIds);

            foreach (var conflictedActivity in conflictedActivities)
            {
                if (conflictedActivity != activity)
                {
                    conflictedActivity.AddConflicts(workerIds);
                }
            }
        }

        public async Task RecalculateConflictsWith(
            Guid activityId,
            DateTimeOffset oldStartTime,
            DateTimeOffset oldEndTime,
            ICollection<char> workerIds)
        {
            var overlappedActivities = 
                await activityRepository.GetConflictedActivities(
                    oldStartTime,
                    oldEndTime,
                    workerIds);

            foreach (var activity in overlappedActivities)
            {
                if (activity.Id != activityId)
                {
                    var overlappedWorkerIds = 
                        await activityRepository.GetOverlappedWorkerIds(
                            activity.StartTime,
                            activity.EndTime,
                            activity.GetWorkerIds(),
                            activity.Id);

                    activity.ResetConflicts(overlappedWorkerIds);
                }
            }
        }

        public async Task Remove(Guid activityId)
        {
            var activity = await activityRepository.Get(activityId);
            if (activity is null)
            {
                return;
            }

            activityRepository.Remove(activity);
            activity.AddDomainEvent(
                new ActivityRemoved(
                    activityId,
                    activity.StartTime,
                    activity.EndTime,
                    activity.GetWorkerIds()));
        }

        public async Task<Activity> Reschedule(
            Guid activityId,
            DateTimeOffset newStartTime,
            DateTimeOffset newEndTime)
        {
            var activity = await activityRepository.Get(activityId);
            if (activity == null)
            {
                throw new InvalidOperationException(
                    $"Activity with id: {activityId} not found.");
            }

            var overlappedWorkerIds = 
                await activityRepository.GetOverlappedWorkerIds(
                    newStartTime,
                    newEndTime,
                    activity.GetWorkerIds(),
                    activityId);

            activity.Reschedule(
                newStartTime,
                newEndTime,
                overlappedWorkerIds);

            activityRepository.Update(activity);
            return activity;
        }

        public async Task<Activity> Schedule(
            DateTimeOffset startTime,
            DateTimeOffset endTime, 
            ActivityType type, 
            ICollection<char> workerIds)
        {
            var overlappedWorkerIds = 
                await activityRepository.GetOverlappedWorkerIds(
                    startTime,
                    endTime,
                    workerIds);

            var activity = 
                new Activity(
                    Guid.NewGuid(),
                    startTime,
                    endTime,
                    type,
                    workerIds,
                    overlappedWorkerIds);

            activityRepository.Create(activity);
            return activity;
        }
    }
}
