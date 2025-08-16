using MassTransit;

namespace Application.Infrastructure.RabbitMqSetup.Registrations;

public class RegistrationState : SagaStateMachineInstance
{
    // Identificador único da instância da saga (chave de correlação)
    public Guid CorrelationId { get; set; }

    // Nome do estado atual (MassTransit grava aqui)
    public string CurrentState { get; set; } = default!;

    // Dados de negócio que a saga precisa manter
    public string? Email { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // (Opcional) Tokens/ids para Schedules/Timeouts
    public Guid? TimeoutTokenId { get; set; }

    // (Opcional) Controle de concorrência (se usar EF otimista)
    public int Version { get; set; }          // ou byte[] RowVersion
}



