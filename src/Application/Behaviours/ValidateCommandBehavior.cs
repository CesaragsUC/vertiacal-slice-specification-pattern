using FluentValidation;

namespace Application.Behaviours;

using Application.Metrics;
using Cortex.Mediator.Commands;
using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

public sealed class ValidateCommandBehavior<TCommand, TResult>
    : ICommandPipelineBehavior<TCommand, TResult>
    where TCommand : class, ICommand<TResult>
{
    private readonly IEnumerable<IValidator<TCommand>> _validators;
    private readonly ILogger<ValidateCommandBehavior<TCommand, TResult>> _logger;
    public ValidateCommandBehavior(IEnumerable<IValidator<TCommand>> validators,
        ILogger<ValidateCommandBehavior<TCommand, TResult>> logger)
    {
        _validators = validators;
        _logger = logger;
    }
        

    public async Task<TResult> Handle(
        TCommand command,
        CommandHandlerDelegate<TResult> next,
        CancellationToken ct)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TCommand>(command);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, ct)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .Select(f => Error.Validation(f.PropertyName, f.ErrorMessage))
            .ToList();

        if (failures.Count > 0)
        {
            // Aqui está o pulo do gato:
            // 'failures' (IEnumerable<Error>) é convertido implicitamente para ErrorOr<T>.
            // O cast via dynamic deixa o runtime escolher T correto (o T de TResult).
            _logger.LogWarning("Validation failed for command {CommandType} with errors: {Errors}",
                typeof(TCommand).Name, string.Join(", ", failures.Select(e => e.Description)));

            foreach (var error in failures?.ToList() ?? [])
            {
                ProductMetrics.ValidationErrors.WithLabels(typeof(TCommand).Name, error.Description,error.Code);
            }

            return (TResult)(dynamic)failures;
        }

        return await next();
    }
}