using Cortex.Mediator.Commands;
using ErrorOr;

public sealed record CreateProductCommand(string Name, decimal Price, bool isActive, Guid CategoryId) : ICommand<ErrorOr<Guid>>;