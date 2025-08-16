using Application;
using Application.Extensions;
using Microsoft.OpenApi.Models;
using Application.Features.MinimalApiDemo;
using Microsoft.Extensions.Configuration;
using Cortex.Mediator.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(AssemblyReference).Assembly); // <-- importante. diz ao ASP.NET Core para procurar controllers (e outros artefatos MVC) em um outro assembly além do assembly da Web API


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

var app = builder.Build();

await app.ApplyMigrationsAsync();


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

app.MapMinimalApiDemoEndpoints();

app.Run();

public partial class Program { }