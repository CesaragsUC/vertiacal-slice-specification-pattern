using Prometheus;
using YourApp.Api.Utils;

namespace YourApp.Api.Configurations;

//https://github.com/prometheus-net/prometheus-net
public static class PrometheusConfig
{
    public static void AddPrometheusConfiguration(this IServiceCollection services)
    {
        // Define an HTTP client that reports metrics about its usage, to be used by a sample background service.
        services.AddHttpClient(SampleService.HttpClientName);

        // Export metrics from all HTTP clients registered in services
        services.UseHttpClientMetrics();

        // A sample service that uses the above HTTP client.
        services.AddHostedService<SampleService>();

        services.AddHealthChecks()
            // Define a sample health check that always signals healthy state.
            .AddCheck<SampleHealthCheck>(nameof(SampleHealthCheck))
            // Report health check results in the metrics output.
            .ForwardToPrometheus();
    }
}
