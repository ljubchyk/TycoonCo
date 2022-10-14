using TycoonCo.Domain;

namespace TycoonCo.Application
{
    public class TycoonCoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IActivityService activityService;
        private readonly IActivityRepository activityRepository;
        private readonly IWorkerRepository workerRepository;

        public TycoonCoService(
            IUnitOfWork unitOfWork,
            IActivityService activityService,
            IActivityRepository activityRepository,
            IWorkerRepository workerRepository)
        {
            this.unitOfWork = unitOfWork;
            this.activityService = activityService;
            this.activityRepository = activityRepository;
            this.workerRepository = workerRepository;
        }

        public async Task<Activity> Schedule(Activity model)
        {
            var type =
                DomainActivityTypeMapper.
                Map(model.Type);

            var workerIds = 
                model.WorkerActivities.
                Select(wa => wa.WorkerId).
                ToArray();

            var activity =
                await activityService.Schedule(
                    model.StartTime,
                    model.EndTime,
                    type,
                    workerIds);

            ActivityMapper.Map(activity, model);
            
            await unitOfWork.Commit();
            return model;
        }

        public async Task<Activity> Reschedule(RescheduleActivityCommand command)
        {
            var activity = 
                await activityService.Reschedule(
                    command.ActivityId,
                    command.NewStartTime,
                    command.NewEndTime);

            var model = ActivityMapper.Map(activity);
            
            await unitOfWork.Commit();
            return model;
        }

        public async Task Remove(Guid activityId)
        {
            await activityService.Remove(activityId);
            await unitOfWork.Commit();
        }

        public async Task<List<Worker>> GetMostBusyWorkers(int limit, int nextDays)
        {
            var workerIds =
                await activityRepository.GetMostBusyWorkers(
                    limit, 
                    nextDays);
            var workers = await workerRepository.GetList(workerIds);

            return workers.Select(w => WorkerMapper.Map(w)).ToList();
        }
    }
}
