using MassTransit;
using TycoonCo.Domain;

namespace TycoonCo.Application.EventHandlers
{
    public class CalculateConflictsOnActivityRescheduled : IConsumer<ActivityRescheduled>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IActivityService activityService;

        public CalculateConflictsOnActivityRescheduled(IUnitOfWork unitOfWork, IActivityService activityService)
        {
            this.unitOfWork = unitOfWork;
            this.activityService = activityService;
        }

        public async Task Consume(ConsumeContext<ActivityRescheduled> context)
        {
            await activityService.RecalculateConflictsWith(
                context.Message.ActivityId,
                context.Message.OldStartTime,
                context.Message.OldEndTime,
                context.Message.WorkerIds);
            await unitOfWork.Commit();
        }
    }
}