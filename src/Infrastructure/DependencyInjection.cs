using Application.Abstractions;
using Application.Infrastructure.Services;
using Infrastructure.Databases;
using Infrastructure.RabbitMqSetup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static  class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IDateTime, DateTimeService>();

        services.AddDbContextConfigurations(configuration);
        services.AddMessageBroker(configuration);

        return services;
    }

}
