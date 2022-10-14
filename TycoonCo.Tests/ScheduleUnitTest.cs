using Microsoft.EntityFrameworkCore;
using TycoonCo.Application;
using TycoonCo.Infrastructure;

namespace TycoonCo.Tests
{
    [TestClass]
    public class ScheduleUnitTest
    {
        private readonly TycoonCoService tycoonCoService;

        public ScheduleUnitTest()
        {
            var activityRepository = new ActivityRepositoryFake();

            tycoonCoService = new TycoonCoService(
                new UnitOfWorkFake(),
                new Domain.ActivityService(activityRepository),
                activityRepository,
                new WorkerRepositoryFake());

            activityRepository.Create(new Domain.Activity(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddHours(1),
                Domain.ActivityType.BuildComponent,
                new[] { 'A' }, 
                new char[0]));

            activityRepository.Create(new Domain.Activity(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow.AddHours(2),
                DateTimeOffset.UtcNow.AddHours(3),
                Domain.ActivityType.BuildComponent,
                new[] { 'A' }, 
                new char[0]));
        }

        [TestMethod]
        public async Task Creates()
        {
            var activity = new Activity
            {
                StartTime = DateTimeOffset.UtcNow.AddHours(-2),
                EndTime = DateTimeOffset.UtcNow.AddHours(-1),
                Type = ActivityType.BuildComponent,
                WorkerActivities = new[] { new WorkerActivity { WorkerId = 'A' } }
            };
            var scheduledActivity = await tycoonCoService.Schedule(activity);

            Assert.AreEqual(activity.EndTime, scheduledActivity.EndTime);
            Assert.AreEqual(activity.StartTime, scheduledActivity.StartTime);
            Assert.AreEqual(activity.Type, scheduledActivity.Type);
            Assert.IsNotNull(scheduledActivity.WorkerActivities);
            Assert.AreEqual(activity.WorkerActivities.Count(), scheduledActivity.WorkerActivities.Count());
            Assert.AreEqual(activity.WorkerActivities.First().WorkerId, scheduledActivity.WorkerActivities.First().WorkerId);
        }

        [TestMethod]
        public async Task CreatesNotConflicted()
        {
            var activity = await tycoonCoService.Schedule(new Activity
            {
                StartTime = DateTimeOffset.UtcNow.AddHours(-2),
                EndTime = DateTimeOffset.UtcNow.AddHours(-1),
                Type = ActivityType.BuildComponent,
                WorkerActivities = new[] { new WorkerActivity { WorkerId = 'A' } }
            });

            Assert.IsFalse(activity.HasConflict);
        }

        [TestMethod]
        public async Task CreatesConflicted()
        {
            var activity = await tycoonCoService.Schedule(new Activity
            {
                StartTime = DateTimeOffset.UtcNow,
                EndTime = DateTimeOffset.UtcNow.AddHours(3),
                Type = ActivityType.BuildComponent,
                WorkerActivities = new[] { new WorkerActivity { WorkerId = 'A' } }
            });

            Assert.IsTrue(activity.HasConflict);
        }

        [TestMethod]
        public async Task FailsForBuildComponentForMultiplyWorkers()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => tycoonCoService.Schedule(new Activity
            {
                StartTime = DateTimeOffset.UtcNow,
                EndTime = DateTimeOffset.UtcNow.AddHours(3),
                Type = ActivityType.BuildComponent,
                WorkerActivities = new[] { new WorkerActivity { WorkerId = 'A' }, new WorkerActivity { WorkerId = 'B' } }
            }));
        }
    }
}