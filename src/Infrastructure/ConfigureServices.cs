using Application.Common.Abstractions;
using Application.Common.Behaviours;
using Application.Infrastructure.Persistence.Contexts;
using Application.Infrastructure.RabbitMqSetup;
using Application.Infrastructure.Services;
using Cortex.Mediator.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCortexMediatRSetup(configuration);
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IDomainEventService, DomainEventService>();
        services.AddTransient<IDateTime, DateTimeService>();


        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextConfigurations(configuration);
        services.AddMessageBroker(configuration);

        return services;
    }


    public static IServiceCollection AddCortexMediatRSetup(this IServiceCollection services, IConfiguration configuration)
    {
        //https://github.com/buildersoftio/cortex/tree/master/src/Cortex.Mediator
        //https://medium.com/@eneshoxha_65350/cortex-mediator-a-free-open-source-alternative-to-mediatr-for-cqrs-in-net-59534e1305c7
        //https://medium.com/@eneshoxha_65350/vertical-slice-architecture-in-net-using-cortex-mediator-and-minimal-apis-dd7fe575d46a
        services.AddCortexMediator(
            configuration,
            new[] { typeof(DependencyInjection) }, // Assemblies to scan
            options => options.AddDefaultBehaviors()
            // faz validação dos commands equery usando Pipeline Behaviors antes de executar o handler
            //Esses Behavior parecem middlewaremas nao são. Eles funcionam dentro do pipeline do (mediator/cortex), não no pipeline HTTP.
            // Sâo executados na ordem que foram adicionados, ou seja, o primeiro a ser adicionado é o primeiro a ser executado.
            .AddOpenCommandPipelineBehavior(typeof(ValidateCommandBehavior<,>))
            .AddOpenQueryPipelineBehavior(typeof(ValidateQueryBehavior<,>))
            //"profiling" simples para detectar handlers lentos, sem alterar os handlers.
            .AddOpenQueryPipelineBehavior(typeof(PerformanceQueryBehavior<,>))
            .AddOpenCommandPipelineBehavior(typeof(PerformanceCommandBehavior<,>))
        );


        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }
}
