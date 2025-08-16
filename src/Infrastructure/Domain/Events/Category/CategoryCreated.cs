namespace Application.Domain.Events.Category;

public sealed record CategoryCreated(Guid Id, string Name, bool IsActive);

