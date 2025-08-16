namespace Application.Common.Behaviours;

using Application.Common.Abstractions;
using Cortex.Mediator.Commands;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

public sealed class PerformanceCommandBehavior<TCommand, TResponse>
    : ICommandPipelineBehavior<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ILogger<TCommand> _logger;
    private readonly ICurrentUserService _currentUser;
    private readonly Stopwatch _timer = new();

    private const int ThresholdMs = 500; // ajuste como quiser

    public PerformanceCommandBehavior(
        ILogger<TCommand> logger,
        ICurrentUserService currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task<TResponse> Handle(
        TCommand command,
        CommandHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        _timer.Restart();

        var response = await next(); // chama o próximo no pipeline / handler

        _timer.Stop();

        var elapsed = _timer.ElapsedMilliseconds;
        if (elapsed > ThresholdMs)
        {
            var userId = _currentUser.UserId ?? string.Empty;
            var name = typeof(TCommand).Name;

            _logger.LogWarning(
                "Cortex Slow Command: {Name} ({Elapsed} ms) {UserId} {@Command}",
                name, elapsed, userId, command);
        }

        return response;
    }
}
