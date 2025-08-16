using Application.Infrastructure.RabbitMqSetup.Registrations;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
namespace Application.Infrastructure.Persistence.Contexts.MTDbContext;

public class RegistrationDbContext : SagaDbContext
{
    public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options) : base(options) { }

    protected override IEnumerable<ISagaClassMap> Configurations
        => new ISagaClassMap[] { new RegistrationStateMap() };


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("saga");
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}