using Prometheus;

namespace Application.Metrics
{
    public static class GlobalMetrics
    {

        public static readonly Histogram CommandDuration = Prometheus.Metrics
            .CreateHistogram(
                "command_duration_seconds",
                "Duração do Command (Cortex command behaviour)",
                new HistogramConfiguration
                {
                    Buckets = new[]
                    {
                                0.005,  // 5ms
                                0.01,   // 10ms
                                0.025,  // 25ms
                                0.05,   // 50ms
                                0.1,    // 100ms
                                0.25,   // 250ms
                                0.5,    // 500ms
                                1.0,    // 1s
                                2.5,    // 2.5s
                                5.0,    // 5s
                                10.0    // 10s
                    }
                });


        public static readonly Histogram QueryDuration = Prometheus.Metrics
            .CreateHistogram(
                "query_duration_seconds",
                "Duração da Query (Cortex query behaviour)",
                new HistogramConfiguration
                {
                    Buckets = new[]
                    {
                                0.005,  // 5ms
                                0.01,   // 10ms
                                0.025,  // 25ms
                                0.05,   // 50ms
                                0.1,    // 100ms
                                0.25,   // 250ms
                                0.5,    // 500ms
                                1.0,    // 1s
                                2.5,    // 2.5s
                                5.0,    // 5s
                                10.0    // 10s
                    }
                });


        public static readonly Histogram DbQueryDuration = Prometheus.Metrics
            .CreateHistogram(
                "database_query_duration_seconds",
                "Database query execution time",
                new HistogramConfiguration
                {
                    LabelNames = new[] { "operation" },
                    Buckets = new[]
                    {
                        0.001,  // 1ms   - Cache hit
                        0.01,   // 10ms  - Índice simples
                        0.05,   // 50ms  - Query simples
                        0.1,    // 100ms - Query normal
                        0.5,    // 500ms - Query complexa
                        1.0,    // 1s    - Query pesada
                        5.0     // 5s    - Timeout próximo
                    }
                });

        public static readonly Histogram HttpDuration = Prometheus.Metrics
            .CreateHistogram(
                "http_request_duration_seconds",
                "HTTP request duration",
                new HistogramConfiguration
                {
                    Buckets = new[]
                    {
                        0.005,  // 5ms
                        0.01,   // 10ms
                        0.025,  // 25ms
                        0.05,   // 50ms
                        0.1,    // 100ms
                        0.25,   // 250ms
                        0.5,    // 500ms
                        1.0,    // 1s
                        2.5,    // 2.5s
                        5.0,    // 5s
                        10.0    // 10s
                    }
                });

        public static readonly Histogram JobDuration = Prometheus.Metrics
            .CreateHistogram(
                "batch_job_duration_seconds",
                "Batch job execution time",
                new HistogramConfiguration
                {
                    Buckets = new[]
                    {
                        1.0,    // 1s
                        5.0,    // 5s
                        10.0,   // 10s
                        30.0,   // 30s
                        60.0,   // 1min
                        300.0,  // 5min
                        600.0,  // 10min
                        1800.0, // 30min
                        3600.0  // 1h
                    }
                });
    }
}
