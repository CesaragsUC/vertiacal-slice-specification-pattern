using Infrastructure.Databases.ApplicationDbContext;
using Infrastructure.Databases.MTDbContext;
using Infrastructure.RabbitMqSetup;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private IConfiguration? _configuration;

    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithDatabase("efspecDbTest")
        .Build();

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3.12-management")
        .WithUsername("guest")
        .WithPassword("guest")
        .WithCommand("rabbitmq-server", "rabbitmq-plugins", "enable", "rabbitmq_management")
        .WithCleanUp(true)
        .Build();

    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithImage("mongo:8.0.12")
        .WithUsername("cesar")
        .WithPassword("cesar")
        .Build();

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        await _rabbitMqContainer.StartAsync().ConfigureAwait(false);
        await _mongoDbContainer.StartAsync().ConfigureAwait(false);

    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        OverrideConfiguration(builder);

        builder.ConfigureTestServices(services =>
        {
            ConfigurePostegreSqlFromTestcontainer(services);
            ConfigureMTDContextSqlFromTestcontainer(services);
            ConfigureMongoDbSqlFromTestcontainer(services);
            ConfigureRabbitMqFromTestcontainer(services);
        });
    }

    private void ConfigurePostegreSqlFromTestcontainer(IServiceCollection services)
    {
        // Remove AppDbContext
        var dbContextDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(AppDbContext));
        if (dbContextDescriptor != null)
        {
            services.Remove(dbContextDescriptor);
        }

        // Remove DbContextOptions<AppDbContext>
        var optionsDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(DbContextOptions<AppDbContext>));
        if (optionsDescriptor != null)
        {
            services.Remove(optionsDescriptor);
        }

        // Remove DbContextOptions (non-generic) if it's related to AppDbContext
        var nonGenericOptionsDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(DbContextOptions));
        if (nonGenericOptionsDescriptor != null)
        {
            services.Remove(nonGenericOptionsDescriptor);
        }

        // Also remove any IDbContextFactory<AppDbContext> if present
        var factoryDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(IDbContextFactory<AppDbContext>));
        if (factoryDescriptor != null)
        {
            services.Remove(factoryDescriptor);
        }

        // Re-add AppDbContext with test container connection string
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
        });
    }

    private void ConfigureMTDContextSqlFromTestcontainer(IServiceCollection services)
    {
        // Remove RegistrationDbContext
        var dbContextDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(RegistrationDbContext));
        if (dbContextDescriptor != null)
        {
            services.Remove(dbContextDescriptor);
        }

        // Remove DbContextOptions<RegistrationDbContext>
        var optionsDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(DbContextOptions<RegistrationDbContext>));
        if (optionsDescriptor != null)
        {
            services.Remove(optionsDescriptor);
        }

        // Also remove any IDbContextFactory<RegistrationDbContext> if present
        var factoryDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(IDbContextFactory<RegistrationDbContext>));
        if (factoryDescriptor != null)
        {
            services.Remove(factoryDescriptor);
        }

        // Re-add RegistrationDbContext with test container connection string
        services.AddDbContext<RegistrationDbContext>(options =>
        {
            options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
        });
    }

    private void ConfigureMongoDbSqlFromTestcontainer(IServiceCollection services)
    {
        // Remove existing MongoClient
        var mongoClientDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IMongoClient));
        if (mongoClientDescriptor != null)
        {
            services.Remove(mongoClientDescriptor);
        }

        // Remove existing IMongoDatabase
        var mongoDatabaseDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IMongoDatabase));
        if (mongoDatabaseDescriptor != null)
        {
            services.Remove(mongoDatabaseDescriptor);
        }

        // Re-add with test container connection string
        services.AddSingleton<IMongoClient>(_ =>
            new MongoClient(_mongoDbContainer.GetConnectionString()));

        services.AddSingleton(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();
            var databaseName = _configuration?.GetSection("Mongo:Database").Value ?? "efspecDbTest";
            return client.GetDatabase(databaseName);
        });
    }

    private void ConfigureRabbitMqFromTestcontainer(IServiceCollection services)
    {
        // Remove original RabbitMq configuration
        RemoveOriginalRabbitMqConfiguration(services);

        // Re-add MassTransit with test container configuration
        services.AddMessageBroker(_configuration!);

    }

    private void RemoveOriginalRabbitMqConfiguration(IServiceCollection services)
    {
        // Remove ALL MassTransit related services
        var servicesToRemove = services.Where(s =>
            s.ServiceType == typeof(IPublishEndpoint) ||
            s.ServiceType == typeof(ISendEndpointProvider) ||
            s.ServiceType == typeof(IBus) ||
            s.ServiceType == typeof(IBusControl) ||
            s.ServiceType.FullName?.Contains("MassTransit") == true ||
            s.ImplementationType?.FullName?.Contains("MassTransit") == true ||
            s.ServiceType.Namespace?.Contains("MassTransit") == true ||
            s.ImplementationType?.Namespace?.Contains("MassTransit") == true
        ).ToList();

        foreach (var service in servicesToRemove)
        {
            services.Remove(service);
        }
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
        await _mongoDbContainer.DisposeAsync();
    }

    private void OverrideConfiguration(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var port = _rabbitMqContainer.GetMappedPublicPort(5672);
            var hostname = _rabbitMqContainer.Hostname;
            var connectionString = _rabbitMqContainer.GetConnectionString();

            var overrideConfig = new Dictionary<string, string?>
            {
                ["Rabbit:Host"] = hostname,
                ["Rabbit:Port"] = port.ToString(),
                ["Rabbit:User"] = "guest",
                ["Rabbit:Pass"] = "guest",
                ["Rabbit:VHost"] = "/",
                ["Mongo:Database"] = "efspecDbTest",
                ["ConnectionStrings:Postgres"] = _postgreSqlContainer.GetConnectionString(),
                ["ConnectionStrings:Mongo"] = _mongoDbContainer.GetConnectionString()
            };

            configBuilder.AddInMemoryCollection(overrideConfig);
            _configuration = configBuilder.Build();
        });
    }
}