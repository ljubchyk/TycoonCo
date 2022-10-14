using MassTransit;
using System.Text.Json;
using TycoonCo.Application.EventHandlers;
using TycoonCo.Domain;

namespace TycoonCo
{
    public static class Extentions
    {
        public static void AddBus(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddMediator(configurator =>
            {
                configurator.AddConsumer<CalculateConflictsOnActivityScheduled>();
                configurator.AddConsumer<CalculateConflictsOnActivityRescheduled>();
                configurator.AddConsumer<CalculateConflictsOnActivityRemoved>();
            });
        }
    }

    //public class DomainEventPublisher : BackgroundService
    //{
    //    private readonly IServiceProvider serviceProvider;
    //    private readonly IBus bus;

    //    public DomainEventPublisher(IServiceProvider serviceProvider, IBus bus)
    //    {
    //        this.serviceProvider = serviceProvider;
    //        this.bus = bus;
    //    }

    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            using var scope = serviceProvider.CreateScope();
    //            var eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
    //            await PublisEvents(eventStore, stoppingToken);
    //            await Task.Delay(500, stoppingToken);
    //        }
    //    }

    //    private async Task PublisEvents(IEventStore eventStore, CancellationToken stoppingToken)
    //    {
    //        if (stoppingToken.IsCancellationRequested)
    //        {
    //            return;
    //        }

    //        var storedEvents = await eventStore.GetList();
    //        foreach (var storedEvent in storedEvents)
    //        {
    //            if (stoppingToken.IsCancellationRequested)
    //            {
    //                return;
    //            }

    //            var domainEventType = typeof(DomainEvent)
    //                .Assembly
    //                .GetType(storedEvent.EventName);
    //            var domainEvent = JsonSerializer.Deserialize(
    //                storedEvent.EventBody,
    //                domainEventType);

    //            await bus.Publish(domainEvent, stoppingToken);
    //            await eventStore.Remove(storedEvent);
    //        }
    //    }
    //}
}