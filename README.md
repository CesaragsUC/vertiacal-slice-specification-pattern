# Vertical Slices — EF Core (Write + Outbox) + PostgreSQL + RabbitMQ + Mongo (Read)

## A basic show case, how to use a hybrid Slice pattern + Specification. Without Repository pattern, a straightforward project.

## Main technologies involved
- EF Core
- Fixture for unit tests
- TestContainer for integration
- Integratiopn Tests
- RabbitMq + Masstransit + Outbox
- Prometheus .Net : https://github.com/prometheus-net/prometheus-net
- Metrics with Prometheus.
- SeqLog to store Serilog : https://datalust.co/
- MongoDb for read
- PostgreSql for write
