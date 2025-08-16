using Application.Common.Models;
using Application.Domain.Events;
using Cortex.Mediator;
using Cortex.Mediator.Notifications;

using Microsoft.Extensions.Logging;

namespace Application.Infrastructure.Services;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}

public class DomainEventService(IMediator mediator, ILogger<DomainEventService> logger) : IDomainEventService
{
    public Task Publish(DomainEvent domainEvent)
    {
        logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
        return mediator.PublishAsync(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    private static INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(
            typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
    }
}