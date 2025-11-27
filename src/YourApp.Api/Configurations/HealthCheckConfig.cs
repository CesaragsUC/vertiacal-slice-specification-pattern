using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

namespace YourApp.Api.Configurations;

public static class HealthCheckConfig
{
    public static void HealthCheckSetup(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    timestamp = DateTime.UtcNow,
                    duration_ms = report.TotalDuration.TotalMilliseconds,
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        description = e.Value.Description,
                        duration_ms = e.Value.Duration.TotalMilliseconds,
                        data = e.Value.Data
                    })
                }, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await context.Response.WriteAsync(result);
            }
        });
    }
}