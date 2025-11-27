

using MassTransit;

namespace Application.Infrastructure.RabbitMqSetup.Registrations;

public record RegistrationSubmitted(Guid RegistrationId, string Email);
public record RegistrationAccepted(Guid RegistrationId);

public class RegistrationStateMachine : MassTransitStateMachine<RegistrationState>
{
    public State Pending { get; private set; } = null!;
    public State Completed { get; private set; } = null!;

    public Event<RegistrationSubmitted> Submitted { get; private set; } = null!;
    public Event<RegistrationAccepted> Accepted { get; private set; } = null!;

    public RegistrationStateMachine()
    {
        // Diz ao MT qual propriedade do state guarda o nome do estado
        InstanceState(x => x.CurrentState);

        // Regras de correlação: como achar a instância da saga a partir da mensagem
        Event(() => Submitted, x => x.CorrelateById(ctx => ctx.Message.RegistrationId));
        Event(() => Accepted, x => x.CorrelateById(ctx => ctx.Message.RegistrationId));

        Initially(
            When(Submitted)
                .Then(ctx =>
                {
                    ctx.Saga.Email = ctx.Message.Email;
                    ctx.Saga.SubmittedAt = DateTime.UtcNow;
                })
                .TransitionTo(Pending)
        );

        During(Pending,
            When(Accepted)
                .Then(ctx => ctx.Saga.CompletedAt = DateTime.UtcNow)
                .TransitionTo(Completed)
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}
