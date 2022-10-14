using MassTransit;
using TycoonCo.Domain;

namespace TycoonCo.Application.EventHandlers
{
    public class CalculateConflictsOnActivityRemoved : IConsumer<ActivityRemoved>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IActivityService activityService;

        public CalculateConflictsOnActivityRemoved(IUnitOfWork unitOfWork, IActivityService activityService)
        {
            this.unitOfWork = unitOfWork;
            this.activityService = activityService;
        }

        public async Task Consume(ConsumeContext<ActivityRemoved> context)
        {
            await activityService.RecalculateConflictsWith(
                context.Message.ActivityId,
                context.Message.StartTime,
                context.Message.EndTime,
                context.Message.WorkerIds);
            await unitOfWork.Commit();
        }
    }
}