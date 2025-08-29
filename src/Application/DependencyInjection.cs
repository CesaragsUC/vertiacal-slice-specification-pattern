using Application.Abstractions;
using Application.Behaviours;
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


        return services;
    }



    public static IServiceCollection AddCortexMediatRSetup(this IServiceCollection services, IConfiguration configuration)
    {
        //https://github.com/buildersoftio/cortex/wiki/12.-Mediator-Design-Pattern#1232-defining-a-command--handler
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
