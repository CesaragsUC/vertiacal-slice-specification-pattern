using Application;
using Application.Abstractions.Data;
using Infrastructure;
using Infrastructure.Databases.ApplicationDbContext;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using Prometheus;
using Serilog;
using YourApp.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); // evita Console/Debug duplicados

// Start Serilog configuration
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

Log.Logger = logger;

builder.Host.UseSerilog(Log.Logger, dispose: true);

builder.Services
    .AddControllers();


builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors(options => options.AddDefaultPolicy(
        policy => policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()));

builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "VSA Todo API", Version = "v1" }));

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<AppDbContext>());

builder.Services.AddPrometheusConfiguration();

var app = builder.Build();


await  app.ApplyMigrationsAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.HealthCheckSetup();
app.MapMetrics();// Capture metrics about all received HTTP requests.

app.Run();

public partial class Program { }