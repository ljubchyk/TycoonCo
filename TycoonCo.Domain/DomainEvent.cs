namespace TycoonCo.Domain
{
    public class DomainEvent
    {
        private readonly DateTime occuredOn;

        public DomainEvent()
        {
            occuredOn = DateTime.UtcNow;
        }

        public DateTime OccuredOn => occuredOn;
    }
}
