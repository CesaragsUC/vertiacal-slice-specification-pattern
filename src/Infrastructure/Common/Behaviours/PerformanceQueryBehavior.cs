namespace Application.Common.Behaviours;

using Application.Common.Abstractions;
using Cortex.Mediator.Queries;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

public sealed class PerformanceQueryBehavior<TQuery, TResponse>
    : IQueryPipelineBehavior<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    private readonly ILogger<TQuery> _logger;
    private readonly ICurrentUserService _currentUser;
    private readonly Stopwatch _timer = new();

    private const int ThresholdMs = 500;

    public PerformanceQueryBehavior(
        ILogger<TQuery> logger,
        ICurrentUserService currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task<TResponse> Handle(
        TQuery query,
        QueryHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        _timer.Restart();

        var response = await next();

        _timer.Stop();

        var elapsed = _timer.ElapsedMilliseconds;
        if (elapsed > ThresholdMs)
        {
            var userId = _currentUser.UserId ?? string.Empty;
            var name = typeof(TQuery).Name;

            _logger.LogWarning(
                "Cortex Slow Query: {Name} ({Elapsed} ms) {UserId} {@Query}",
                name, elapsed, userId, query);
        }

        return response;
    }
}
