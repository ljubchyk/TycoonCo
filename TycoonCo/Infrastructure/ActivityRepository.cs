using Microsoft.EntityFrameworkCore;
using TycoonCo.Domain;

namespace TycoonCo.Infrastructure
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly Db db;

        public ActivityRepository(Db db)
        {
            this.db = db;
        }

        public void Create(Activity activity)
        {
            db.Add(activity);
        }

        public Task<Activity> Get(Guid id)
        {
            return db.Activities.FindAsync(id).AsTask();
        }

        public Task<List<char>> GetMostBusyWorkers(int limit, int nextDays)
        {
            var from = DateTime.UtcNow.Date.AddDays(1);
            var to = DateTime.UtcNow.Date.AddDays(nextDays);
            return db.Activities
                .Where(a => a.EndTime > from && a.StartTime < to)
                .SelectMany(a => a.WorkerActivities.Select(w => new
                {
                    WorkerId = w.WorkerId,
                    Ticks = Math.Min(a.EndTime.Ticks, to.Ticks) - Math.Max(a.StartTime.Ticks, from.Ticks)
                }))
                .GroupBy(i => i.WorkerId)
                .Select(g => new
                {
                    WorkerId = g.Key,
                    Ticks = g.Sum(t => t.Ticks)
                })
                .OrderByDescending(i => i.Ticks)
                .Take(limit)
                .Select(i => i.WorkerId)
                .ToListAsync();
        }

        public Task<List<Activity>> GetConflictedActivities(DateTimeOffset startTime, DateTimeOffset endTime, ICollection<char> workerIds)
        {
            return db.Activities
                .Where(a => a.WorkersEndTime >= startTime && a.StartTime <= endTime && a.WorkerActivities.Any(wa => workerIds.Contains(wa.WorkerId)))
                .ToListAsync();
        }

        public void Remove(Activity activity)
        {
            db.Remove(activity);
        }

        public void Update(Activity activity)
        {
            db.Update(activity);
        }

        public void Update(ICollection<Activity> activities)
        {
            db.UpdateRange(activities);
        }

        public Task<List<char>> GetOverlappedWorkerIds(
            DateTimeOffset startTime, 
            DateTimeOffset endTime,
            ICollection<char> workerIds,
            Guid? outcludedActivityId = null)
        {
            return db.Activities
               .Where(a =>
                (outcludedActivityId == null || a.Id != outcludedActivityId) &&
                a.WorkersEndTime >= startTime &&
                a.StartTime <= endTime &&
                a.WorkerActivities.Any(wa => workerIds.Contains(wa.WorkerId)))
               .SelectMany(a => a.WorkerActivities.Select(wa => wa.WorkerId))
               .Distinct()
               .ToListAsync();
        }
    }
}