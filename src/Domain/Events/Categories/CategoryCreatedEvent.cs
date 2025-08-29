namespace Application.Domain.Events.Category;

public sealed record CategoryCreatedEvent(Guid Id, string Name, bool IsActive);

