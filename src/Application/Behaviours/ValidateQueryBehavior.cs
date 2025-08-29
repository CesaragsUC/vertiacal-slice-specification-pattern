namespace Application.Behaviours;

using Cortex.Mediator.Queries;
using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.Logging;

public sealed class ValidateQueryBehavior<TQuery, TResult>
   : IQueryPipelineBehavior<TQuery, TResult>
   where TQuery : class, IQuery<TResult>
{
    private readonly IEnumerable<IValidator<TQuery>> _validators;
    private readonly ILogger<ValidateQueryBehavior<TQuery, TResult>> _logger;

    public ValidateQueryBehavior(IEnumerable<IValidator<TQuery>> validators,
        ILogger<ValidateQueryBehavior<TQuery, TResult>> logger)
    {
        _validators = validators;
        _logger = logger; // Assign ILogger
    }

    public async Task<TResult> Handle(
        TQuery query,
        QueryHandlerDelegate<TResult> next,
        CancellationToken ct)
    {
        if (!_validators.Any())
            return await next();

        var ctx = new ValidationContext<TQuery>(query);
        var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(ctx, ct)));

        var failures = results.SelectMany(r => r.Errors)
                              .Where(e => e is not null)
                              .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage))
                              .ToList();

        if (failures.Count > 0)
        {
            _logger.LogWarning("Validation failed for query {QueryType} with errors: {Errors}",
                typeof(TQuery).Name, string.Join(", ", failures.Select(e => e.Description)));

            return (TResult)(dynamic)failures; // converte IEnumerable<Error> -> ErrorOr<T>
        }

        return await next();
    }
}
