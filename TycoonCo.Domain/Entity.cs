namespace TycoonCo.Domain
{
    public abstract class Entity<T> : Entity, IEquatable<Entity<T>>
    {
        protected readonly T id;

        protected Entity()
        {
        }

        protected Entity(T id)
            : this()
        {
            this.id = id;
        }

        public T Id => id;

        public bool Equals(Entity<T> other)
        {
            if (other is null)
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<T>);
        }
    }

    public class Entity
    {
        private readonly List<DomainEvent> domainEvents;

        protected Entity()
        {
            domainEvents = new List<DomainEvent>();
        }

        public IReadOnlyList<DomainEvent> DomainEvents =>
            domainEvents;

        public void AddDomainEvent(DomainEvent domainEvent)
        {
            domainEvents.Add(domainEvent);
        }
    }
}
