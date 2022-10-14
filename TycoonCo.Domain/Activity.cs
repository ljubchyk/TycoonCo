using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TycoonCo.Domain
{
    public class Activity : Entity<Guid>
    {
        private DateTimeOffset startTime;
        private DateTimeOffset endTime;
        private DateTimeOffset workersEndTime;
        private bool hasConflict;
        private readonly ActivityType type;
        private readonly HashSet<WorkerActivity> workerActivities;

        private Activity()
        {

        }

        public Activity(
            Guid id, 
            DateTimeOffset startTime, 
            DateTimeOffset endTime, 
            ActivityType type, 
            ICollection<char> workerIds, 
            ICollection<char> overlappedWorkerIds)
                    : base(id)
        {
            if (startTime >= endTime)
            {
                throw new ArgumentException("Activity's startTime must be before endTime.", nameof(startTime));
            }

            if (workerIds is null)
            {
                throw new ArgumentException("Activity's workerIds must not be null.", nameof(workerIds));
            }

            if (!workerIds.Any())
            {
                throw new ArgumentException("Activity's workerIds must not be empty.", nameof(workerIds));
            }

            if (type == ActivityType.BuildComponent && workerIds.Count > 1)
            {
                throw new ArgumentException("Activity's workerIds must be of one element.", nameof(workerIds));
            }

            this.startTime = startTime;
            this.endTime = endTime;
            this.type = type;
            workerActivities = new HashSet<WorkerActivity>(
                workerIds.Select(workerId => new WorkerActivity(workerId)));
            CalculateWorkersEndTime();

            AddConflicts(overlappedWorkerIds);
            AddDomainEvent(new ActivityScheduled(id, startTime, endTime, GetWorkerIds()));
        }

        public DateTimeOffset StartTime => startTime;
        public DateTimeOffset EndTime => endTime;
        public DateTimeOffset WorkersEndTime => workersEndTime;
        public ActivityType Type => type;
        public bool HasConflict => hasConflict;
        public IReadOnlySet<WorkerActivity> WorkerActivities => workerActivities;

        internal void AddConflicts(ICollection<char> workerIds)
        {
            foreach (var workerId in workerIds)
            {
                var workerActivity = workerActivities.FirstOrDefault(
                    workerActivity => workerActivity.WorkerId == workerId);
                if (workerActivity is not null)
                {
                    workerActivity.AddConflict();
                    hasConflict = true;
                }
            }
        }

        internal void ResetConflicts(ICollection<char> workerIds)
        {
            foreach (var workerActivity in workerActivities)
            {
                workerActivity.RemoveConflict();
            }
            hasConflict = false;

            AddConflicts(workerIds);
        }

        public void Reschedule(
            DateTimeOffset startTime,
            DateTimeOffset endTime,
            ICollection<char> overlappedWorkerIds)
        {
            if (startTime >= endTime)
            {
                throw new ArgumentException("Activity's startTime must be before endTime.", nameof(startTime));
            }

            var oldStartTime = this.startTime;
            var oldEndTime = this.EndTime;

            this.startTime = startTime;
            this.endTime = endTime;
            
            CalculateWorkersEndTime();
            ResetConflicts(overlappedWorkerIds);

            AddDomainEvent(new ActivityRescheduled(
                Id,
                startTime,
                endTime,
                oldStartTime,
                oldEndTime,
                GetWorkerIds()));
        }

        public List<char> GetWorkerIds()
        {
            return workerActivities.Select(wa => wa.WorkerId).ToList();
        }

        private void CalculateWorkersEndTime()
        {
            workersEndTime = type switch
            {
                ActivityType.BuildComponent => endTime + TimeSpan.FromHours(2),
                ActivityType.BuildMachine => endTime + TimeSpan.FromHours(4),
                _ => throw new NotSupportedException("Unknown ActivityType."),
            };
        }
    }
}
