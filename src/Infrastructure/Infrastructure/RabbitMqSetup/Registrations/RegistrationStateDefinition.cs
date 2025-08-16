namespace Application.Infrastructure.RabbitMqSetup.Registrations;

using Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
using MassTransit;

public class RegistrationStateDefinition :
    SagaDefinition<RegistrationState>
{
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<RegistrationState> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 500, 1000));

        endpointConfigurator.UseEntityFrameworkOutbox<AppDbContext>(context);
    }
}
