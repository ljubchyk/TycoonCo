using TycoonCo.Application;

namespace TycoonCo.Tests
{
    [TestClass]
    public class RescheduleUnitTest
    {
        private readonly Guid activityId;
        private readonly TycoonCoService tycoonCoService;

        public RescheduleUnitTest()
        {
            var activityRepository = new ActivityRepositoryFake();

            activityId = Guid.NewGuid();    

            tycoonCoService = new TycoonCoService(
                new UnitOfWorkFake(),
                new Domain.ActivityService(activityRepository),
                activityRepository,
                new WorkerRepositoryFake());

            activityRepository.Create(new Domain.Activity(
                activityId,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddHours(1),
                Domain.ActivityType.BuildComponent,
                new[] { 'A' },
                new char[0]));

            activityRepository.Create(new Domain.Activity(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow.AddDays(1),
                DateTimeOffset.UtcNow.AddDays(1).AddHours(1),
                Domain.ActivityType.BuildComponent,
                new[] { 'A' },
                new char[0]));
        }

        [TestMethod]
        public async Task RescheduledWithConflict()
        {
            var newStartTime = DateTimeOffset.UtcNow.AddDays(1);
            var newEndTime = DateTimeOffset.UtcNow.AddDays(2);

            var activity = await tycoonCoService.Reschedule(
                new RescheduleActivityCommand
                {
                    ActivityId = activityId,
                    NewStartTime = newStartTime,
                    NewEndTime = newEndTime
                });

            Assert.IsTrue(activity.HasConflict);
        }

        [TestMethod]
        public async Task RescheduledWithoutConflict()
        {
            var newStartTime = DateTimeOffset.UtcNow.AddHours(1);
            var newEndTime = DateTimeOffset.UtcNow.AddHours(2);

            var activity = await tycoonCoService.Reschedule(
                new RescheduleActivityCommand
                {
                    ActivityId = activityId,
                    NewStartTime = newStartTime,
                    NewEndTime = newEndTime
                });

            Assert.IsFalse(activity.HasConflict);
        }

        [TestMethod]
        public async Task Rescheduled()
        {
            var newStartTime = DateTimeOffset.UtcNow.AddHours(1);
            var newEndTime = DateTimeOffset.UtcNow.AddHours(2);

            var activity = await tycoonCoService.Reschedule(
                new RescheduleActivityCommand
                {
                    ActivityId = activityId,
                    NewStartTime = newStartTime,
                    NewEndTime = newEndTime
                });

            Assert.AreEqual(activity.EndTime, newEndTime);
            Assert.AreEqual(activity.StartTime, newStartTime);
        }
    }
}