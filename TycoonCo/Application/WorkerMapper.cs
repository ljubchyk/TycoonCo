namespace TycoonCo.Application
{
    public static class WorkerMapper
    {
        public static Worker Map(Domain.Worker worker)
        {
            return new Worker
            {
                Id = worker.Id
            };
        }
    }
}
