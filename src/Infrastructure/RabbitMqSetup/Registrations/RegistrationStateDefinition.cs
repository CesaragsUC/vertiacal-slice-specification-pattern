namespace Application.Infrastructure.RabbitMqSetup.Registrations;

using global::Infrastructure.Databases.ApplicationDbContext;
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
