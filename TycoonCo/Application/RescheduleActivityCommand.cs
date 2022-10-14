namespace TycoonCo.Application
{
    public class RescheduleActivityCommand
    {
        public Guid ActivityId { get; set; }
        public DateTimeOffset NewStartTime { get; set; }
        public DateTimeOffset NewEndTime { get; set; }
    }
}
