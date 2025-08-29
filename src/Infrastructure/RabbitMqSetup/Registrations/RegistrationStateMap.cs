using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Application.Infrastructure.RabbitMqSetup.Registrations;

// Mapeamento do State
public class RegistrationStateMap : SagaClassMap<RegistrationState>
{
    protected override void Configure(EntityTypeBuilder<RegistrationState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState).HasMaxLength(64);
        entity.Property(x => x.Email).HasMaxLength(256);
    }
}