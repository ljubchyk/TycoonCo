namespace TycoonCo.Application
{
    public static class DomainActivityTypeMapper
    {
        public static Domain.ActivityType Map(ActivityType from)
        {
            return from switch
            {
                ActivityType.BuildComponent => Domain.ActivityType.BuildComponent,
                ActivityType.BuildMachine => Domain.ActivityType.BuildMachine,
                _ => throw new NotSupportedException($"Unknown ActivityType: {from}"),
            };
        }
    }
}
