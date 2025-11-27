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
- 
## Project Configuration Files

### `.editorconfig`
Defines coding styles and formatting rules enforced across all files in the solution. Ensures consistent code style, naming conventions, and modern C# patterns across the entire team.

### `Directory.Build.props`
Centralized MSBuild properties for all projects in the solution. Contains build settings, compiler options, warning configurations, and common assembly metadata.

### `Directory.Packages.props`
Central package version management using MSBuild's `ManagePackageVersionsCentrally` feature. Defines NuGet package versions in a single location for the entire solution.
