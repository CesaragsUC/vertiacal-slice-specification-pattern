namespace Infrastructure.Consumers;

using Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
using Infrastructure.Databases.ApplicationDbContext;
using MassTransit;

public sealed class ProductCreatedConsumerDefinition
    : ConsumerDefinition<ProductCreatedConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ProductCreatedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // Caso esgotar ambos, a mensagem vai para a _error (DLQ).
        // 3 tentativas rápidas (curto prazo)
        endpointConfigurator.UseMessageRetry(r => r.Intervals(
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromSeconds(1)));

        // Se ainda falhar, tenta de novo depois (médio prazo)
        endpointConfigurator.UseDelayedRedelivery(r => r.Intervals(
            TimeSpan.FromSeconds(10),
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(5)));

        // Configura o Outbox para esse consumidor
        endpointConfigurator.UseEntityFrameworkOutbox<AppDbContext>(context);
    }
}
