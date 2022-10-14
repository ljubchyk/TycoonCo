using MassTransit.Mediator;
using TycoonCo.Application;
using TycoonCo.Domain;

namespace TycoonCo.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Db db;
        private readonly IMediator mediator;

        public UnitOfWork(Db db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        public async Task Commit()
        {
            await db.SaveChangesAsync();

            //use outbox
            foreach (var entry in db.ChangeTracker.Entries())
            {
                foreach (var domainEvent in ((Entity)entry.Entity).DomainEvents)
                {
                    mediator.Send(domainEvent);
                }
            }
        }
    }
}