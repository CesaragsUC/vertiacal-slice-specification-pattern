using Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
using Application.Infrastructure.Persistence.Contexts.MTDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Application.Infrastructure.Persistence.Contexts;

public static class DbContextExtensions
{
    public static IServiceCollection AddDbContextConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
             opt.UseNpgsql(configuration.GetConnectionString("Postgres") ?? "Host=localhost;Port=5432;Database=yourapp;Username=postgres;Password=postgres"));

        services.AddDbContext<RegistrationDbContext>(o =>
              o.UseNpgsql(configuration.GetConnectionString("Postgres") ?? "Host=localhost;Port=5432;Database=yourapp;Username=postgres;Password=postgres"));

        services.AddSingleton<IMongoClient>(_ =>
            new MongoClient(configuration.GetConnectionString("Mongo") ?? "mongodb://localhost:27017"));

        services.AddSingleton(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();
            var databaseName = configuration.GetSection("Mongo:Database").Value ?? "yourapp";
            return client.GetDatabase(databaseName);
        });

        return services;
    }
}
