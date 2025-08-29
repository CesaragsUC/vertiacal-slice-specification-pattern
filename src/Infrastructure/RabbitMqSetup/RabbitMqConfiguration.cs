using Application.Infrastructure.RabbitMqSetup;
using Application.Infrastructure.RabbitMqSetup.Registrations;
using Infrastructure.Consumers;
using Infrastructure.Databases.ApplicationDbContext;
using Infrastructure.Databases.MTDbContext;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.RabbitMqSetup;

public static class RabbitMqConfiguration
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProductCreatedConsumer, GlobalConsumerDefinition<ProductCreatedConsumer>>();
            x.AddConsumer<ProductDeletedConsumer, GlobalConsumerDefinition<ProductDeletedConsumer>>();
            x.AddConsumer<ProductUpdatedConsumer, GlobalConsumerDefinition<ProductUpdatedConsumer>>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetSection("Rabbit:Host").Value ?? "localhost", "/", h =>
                {
                    h.Username(configuration.GetSection("Rabbit:User").Value ?? "guest");
                    h.Password(configuration.GetSection("Rabbit:Pass").Value ?? "guest");
                });

                cfg.ConfigureEndpoints(context);
            });

            // Transactional Outbox no EF + Bus Outbox
            x.AddEntityFrameworkOutbox<AppDbContext>(o =>
            {
                o.UsePostgres();   // ou SqlServer/MySql
                o.UseBusOutbox();  // intercepta Publish/Send fora de consumer (ex.: controller/handler)
            });

            x.AddSagaStateMachine<RegistrationStateMachine, RegistrationState, RegistrationStateDefinition>()
            .EntityFrameworkRepository(r =>
            {
                r.ExistingDbContext<RegistrationDbContext>();
                r.UsePostgres();
                r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
            });

        });

        return services;
    }
}
