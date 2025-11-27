using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace YourApp.Api.Utils;

public sealed class SampleHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Verifica uso de memória
            var allocatedMB = GC.GetTotalMemory(false) / 1024 / 1024;

            // Verifica threads
            var threadCount = Process.GetCurrentProcess().Threads.Count;

            // Tempo de atividade
            var uptime = DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime();

            var data = new Dictionary<string, object>
                {
                    { "memory_mb", allocatedMB },
                    { "thread_count", threadCount },
                    { "uptime_hours", Math.Round(uptime.TotalHours, 2) },
                    { "cpu_time_seconds", Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds },
                    { "timestamp", DateTime.UtcNow }
                };

            // Lógica de saúde
            if (allocatedMB > 1024) // Mais de 1GB
            {
                return Task.FromResult(
                    HealthCheckResult.Unhealthy(
                        $"Memory usage too high: {allocatedMB}MB",
                        data: data));
            }

            if (allocatedMB > 512) // Entre 512MB e 1GB
            {
                return Task.FromResult(
                    HealthCheckResult.Degraded(
                        $"Memory usage elevated: {allocatedMB}MB",
                        data: data));
            }

            return Task.FromResult(
                HealthCheckResult.Healthy(
                    $"Application healthy - {allocatedMB}MB memory, {threadCount} threads",
                    data: data));
        }
        catch (Exception ex)
        {
            return Task.FromResult(
                HealthCheckResult.Unhealthy(
                    "Failed to check application health",
                    exception: ex));
        }
    }

}
