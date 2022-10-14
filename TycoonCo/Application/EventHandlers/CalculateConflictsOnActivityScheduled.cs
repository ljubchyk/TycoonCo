using MassTransit;
using TycoonCo.Domain;

namespace TycoonCo.Application.EventHandlers
{
    public class CalculateConflictsOnActivityScheduled : IConsumer<ActivityScheduled>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IActivityService activityService;

        public CalculateConflictsOnActivityScheduled(IUnitOfWork unitOfWork, IActivityService activityService)
        {
            this.unitOfWork = unitOfWork;
            this.activityService = activityService;
        }

        public async Task Consume(ConsumeContext<ActivityScheduled> context)
        {
            await activityService.AddConflictsWith(context.Message.ActivityId);
            await unitOfWork.Commit();
        }
    }
}