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

## ⚙️ Project Configuration Files

### 🎨 [`.editorconfig`](https://editorconfig.org/)
Defines coding styles and formatting rules enforced across all files in the solution. Ensures consistent code style, naming conventions, and modern C# patterns across the entire team.

**Key features:**
- C# 12 modern syntax enforcement
- Naming conventions (interfaces, private fields, async methods)
- Pattern matching and null-checking preferences
- 150+ code quality rules enabled

### 🔧 [`Directory.Build.props`](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-by-directory)
Centralized MSBuild properties for all projects in the solution. Contains build settings, compiler options, warning configurations, and common assembly metadata.

**Key features:**
- `TreatWarningsAsErrors` enabled for maximum code quality
- `WarningLevel` set to 5 (strictest)
- Deterministic builds for CI/CD reproducibility
- Automatic test project detection

### 📦 [`Directory.Packages.props`](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
Central package version management using MSBuild's `ManagePackageVersionsCentrally` feature. Defines NuGet package versions in a single location for the entire solution.

**Benefits:**
- Eliminates version conflicts
- Simplifies dependency updates
- Provides clear audit trail of package versions
- Reduces `.csproj` file clutter

