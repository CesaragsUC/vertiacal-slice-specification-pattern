using Prometheus;
using System.Diagnostics.Metrics;


namespace Application.Metrics;


public static class ProductMetrics
{
    public static readonly Counter ProductsCreated = Prometheus.Metrics
        .CreateCounter("products_created_total", "Total de produtos criados",
            new CounterConfiguration { LabelNames = new[] { "status" } });

    public static readonly Counter ProductsUpdated = Prometheus.Metrics
    .CreateCounter("products_updated_total", "Total de produtos atualizados",
        new CounterConfiguration { LabelNames = new[] {  "status" } });

    public static readonly Counter ProductsDeleted = Prometheus.Metrics
    .CreateCounter("products_deleted_total", "Total de produtos deletados",
        new CounterConfiguration { LabelNames = new[] { "status" } });

    public static readonly Counter ProductNotFound = Prometheus.Metrics
    .CreateCounter("product_not_foun_total", "Produtos nao encontrados",
        new CounterConfiguration { LabelNames = new[] { "error_type" } });


    public static readonly Counter ValidationErrors = Prometheus.Metrics
        .CreateCounter("product_validation_errors_total", "Erros de validação",
            new CounterConfiguration { LabelNames = new[] { "operation", "error_type" } });

    public static readonly Counter ValidationFailures = Prometheus.Metrics
    .CreateCounter(name: "product_validation_failures_total", "Total de validações que falharam");

    public static readonly Histogram ProcessDuration = Prometheus.Metrics
    .CreateHistogram("process_duration_seconds", "Duração da opercao");


    // Histogram - Distribuição de quantidade de erros
    public static readonly Histogram ValidationErrorCount = Prometheus.Metrics
        .CreateHistogram(name: "product_validation_error_count", "Quantidade de erros por validação");

    public static readonly Gauge ActiveProducts = Prometheus.Metrics
        .CreateGauge(
            "active_products_by_category",
            "Número de produtos ativos por categoria",
            new GaugeConfiguration
            {
                LabelNames = new[] { "category" }
            });
}